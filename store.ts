import { computed, effect, inject, signal, untracked } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { injectFilteredReferentialQuery } from '@crew/shared/api';
import { ClientOptions } from '@crew/shared/component';
import {
  OptionModel,
  PaginatedQueryParams,
  PI_SIMUL_MESSAGE,
  SuccessResponse,
} from '@crew/shared/model';
import { SnackBarService, withReferentials } from '@crew/shared/store';
import {
  codeAndLabelReferentialEntityParser,
  codeAsBooleanReferentialEntityParser,
  codeAsNumberAndLabelReferentialEntityParser,
  codeColonLabelReferentialEntityParser,
  codeReferentialEntityParser,
  formattedLabelReferentialEntityParser,
  formattedLabelReferentialEntityParserWithoutNumberCode,
  removeNullishProperties,
} from '@crew/shared/util-common';
import { updateControl, valueSignalFromControl } from '@crew/shared/util-form';
import { SIMULATION_MANAGER } from '@crew-shared-feature/simulation/token/simulation-token';
import {
  patchState,
  signalStore,
  withComputed,
  withHooks,
  withMethods,
  withProps,
  withState,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { filter, pipe, tap } from 'rxjs';

import {
  injectPiClientCreateCommand,
  injectPiClientGetQuery,
  injectPiClientUpdateCommand,
} from '../api/pi-client-form.query';
import { injectPiClientListGetQuery } from '../api/pi-client-list.query';
import {
  fromPiSimulationClientDto,
  LocalIdModel,
  PiSimulationClientDto,
  toPiSimulationClientDto,
  UpsertPiSimulationClientRequestDto,
} from '../model/pi-client-dto';
import { injectPiSimulationClientForm } from './pi-client-form-injector';

//#region referential mapping & parser
export const piSimulationClientReferentialsMapping = {
  linkTypeList: 'LinkType',
  entityList: 'Entity',
  casaEntityList: 'CasaEntity',
  individualRiskList: 'IndividualRisksPoles',
  organizationTypeList: 'OrganizationType',
  kycStatusList: 'KYCStatus',

  korusRatingList: 'InternalRating',
  internalRatingList: 'InternalRating',
  ratingMethodologyList: 'RatingMethodology',
  ratingReasonList: 'RatingReason',
  ratingToolList: 'RatingTool',
  turnoverTypeList: 'EuTurnoverType',
  currencyList: 'Currency',

  doubtfulAttributesList: 'DoubtfulAttributes',
  businessAreaList: 'BusinessArea',
  nafCodeList: 'NAFCodesRev2',
  countryList: 'Country',
  yesNoList: 'YesNoDB',
  clientCategoryList: 'ClientCategory',
  biiPortfolioClientList: 'BIIPortfolioClient',
  actorTypeList: 'ActorType',
  commitmentTypeList: 'ClientCommitmentType',
  lgdBankModifierList: 'LGDBankModifier',
  lgdFundModifierList: 'LGDFundModifier',
  aliasType: 'AliasType',
};

export const piSimulationClientReferentialsEntityParser = {
  entityList: formattedLabelReferentialEntityParserWithoutNumberCode(4),
  casaEntityList: codeAndLabelReferentialEntityParser,
  individualRiskList: codeAndLabelReferentialEntityParser,
  kycStatusList: formattedLabelReferentialEntityParserWithoutNumberCode(2),

  korusRatingList: codeColonLabelReferentialEntityParser,
  internalRatingList: codeReferentialEntityParser,
  ratingMethodologyList: codeAndLabelReferentialEntityParser,
  ratingReasonList: codeAsNumberAndLabelReferentialEntityParser,
  turnoverTypeList: codeAsNumberAndLabelReferentialEntityParser,
  currencyList: codeReferentialEntityParser,

  doubtfulAttributesList: codeAndLabelReferentialEntityParser,
  businessAreaList: codeAndLabelReferentialEntityParser,
  nafCodeList: codeAndLabelReferentialEntityParser,
  countryList: codeAndLabelReferentialEntityParser,
  yesNoList: codeAsBooleanReferentialEntityParser,
  clientCategoryList: codeAndLabelReferentialEntityParser,
  biiPortfolioClientList: formattedLabelReferentialEntityParser(3),
  actorTypeList: codeAsNumberAndLabelReferentialEntityParser,
  commitmentTypeList: codeAndLabelReferentialEntityParser,
  lgdBankModifierList: codeAsNumberAndLabelReferentialEntityParser,
  lgdFundModifierList: codeAndLabelReferentialEntityParser,
};
//#endregion

export const PiSimulationClientStore = signalStore(
  { protectedState: false },

  withReferentials(
    ['clientFormReferentials', 'loadClientFormReferentials'],
    piSimulationClientReferentialsMapping,
    piSimulationClientReferentialsEntityParser,
  ),

  withState({
    creditFileId: null as number | null,
    clientId: null as number | null,
    deviceId: null as number | null,
    korusId: null as number | null,
    kycId: null as number | null,
    clientName: null as string | null,
    fileName: null as string | null,
    localIds: [] as LocalIdModel[],
    isExternal: false, // true when client added as guarantee/depository in mitigant
    clientType: ClientOptions.DEFAULT as ClientOptions,
    isReferentialsPending: true,
    queryAutoFetchDisabled: false,
    isPageIntiailizeFailed: false,
    isPageInitializing: true,
  }),

  withProps(() => ({
    simulationManager: inject(SIMULATION_MANAGER),
  })),

  withComputed((store) => {
    const filteredRefQuery = injectFilteredReferentialQuery(
      signal(['BusinessArea']), //list of ref type
      signal(true),
    );
    const getAllClientsQueryParam = computed(
      () =>
        removeNullishProperties({
          id: store.creditFileId(),
          pageIndex: 0,
          pageSize: 0,
          sortBy: 'id',
          isAscending: true,
        }) as PaginatedQueryParams<{ id: number }>,
    );
    const getAllClientsQuery = injectPiClientListGetQuery(
      getAllClientsQueryParam,
    );
    const computedGetAllClientsQuery = computed(() => getAllClientsQuery);
    const clientList = computed(
      () => computedGetAllClientsQuery().data()?.data ?? [],
    );
    const existingAttachedKycList = computed(() =>
      clientList()
        .filter((client) => client.kycId)
        .map((client) => client.kycId),
    );

    const form = injectPiSimulationClientForm();
    const detailsForm = form.controls.details;
    const ratingForm = form.controls.ratings;
    const riskForm = form.controls.risks;

    const formValue = toSignal(form.valueChanges, { initialValue: form.value });

    const formStatus = toSignal(form.statusChanges, {
      initialValue: form.status,
    });
    const isFormInValid = computed(() => formStatus() === 'INVALID');

    const detailsFormStatus = toSignal(detailsForm.statusChanges, {
      initialValue: detailsForm.status,
    });
    const isDetailsFormInvalid = computed(
      () => detailsFormStatus() === 'INVALID',
    );

    const ratingFormStatus = toSignal(ratingForm.statusChanges, {
      initialValue: ratingForm.status,
    });
    const isRatingFormInvalid = computed(
      () => ratingFormStatus() === 'INVALID',
    );
    const riskFormStatus = toSignal(riskForm.statusChanges, {
      initialValue: riskForm.status,
    });
    const isRiskFormInvalid = computed(() => riskFormStatus() === 'INVALID');

    const referentials = store.clientFormReferentials().data.referentials;
    const businessAreaList = computed(() =>
      referentials
        .businessAreaList()
        .filter((x) => x.additionalField['Level'] === '2'),
    );
    const categoryOfCounterpartyList = computed(() =>
      referentials
        .clientCategoryList()
        .filter((x) => x.additionalField['Level'] === '3'),
    );
    const [businessAreaControlValue, korusIdControlValue, kycIdControlValue] =
      valueSignalFromControl([
        riskForm.controls.businessAreaCode,
        detailsForm.controls.korusId,
        detailsForm.controls.kycIdentifier,
      ]);

    const nafCodeList = computed(
      () =>
        filteredRefQuery
          .data()
          ?.data.find((x) => x.referentialType === 'BusinessArea')
          ?.items.find((y) => y.code === businessAreaControlValue())
          ?.children.find((x) => x.referentialType === 'NAFCodesRev2')
          ?.items.map((y) => {
            return {
              code: y.code,
              label: `${y.code} - ${y.label}`,
              additionalField: {},
            } as OptionModel;
          }) ?? [],
    );

    const isAttachedWithKorus = computed(() => !!korusIdControlValue());

    const isUpdatedWithKyc = computed(() => !!kycIdControlValue());

    const isUpdateFromKycHidden = computed(
      () => !!korusIdControlValue() || !!kycIdControlValue(),
    );
    /** INFO: ClientOptions is MANUAL, tanstack query should not be fetching */
    const queryParams = computed(() => {
      return removeNullishProperties({
        clientId:
          store.clientType() === ClientOptions.STORED ? store.clientId() : null,
        korusId:
          store.clientType() === ClientOptions.KORUS ? store.clientId() : null,
        kycId:
          store.clientType() === ClientOptions.KYC ? store.clientId() : null,
      });
    });

    const isComputeOnSaveDialogOpened = signal(false);

    const getPiClientQuery = signal(
      injectPiClientGetQuery(queryParams, store.isReferentialsPending),
    );

    return {
      isReadOnly: store.simulationManager.isReadOnly,
      existingAttachedKycList,
      businessAreaList,
      categoryOfCounterpartyList,
      nafCodeList,
      /** forEdit is true when in modify mode */
      forEdit: computed(() => {
        const clientId = store.clientId();
        if (
          store.clientType() === ClientOptions.KORUS ||
          store.clientType() === ClientOptions.KYC ||
          store.clientType() === ClientOptions.MANUAL ||
          !clientId ||
          clientId < 0
        ) {
          return false;
        }
        return true;
      }),
      isAttachedWithKorus,
      isUpdatedWithKyc,
      korusIdViewValue: computed(() =>
        korusIdControlValue()?.toString().padStart(10, '0'),
      ),
      isUpdateFromKycHidden,
      localIdList: signal<(LocalIdModel | undefined)[]>([]),
      queryParams,
      getPiClientQuery,
      form: computed(() => form),
      formValue,
      formStatus,
      isFormInValid,
      detailsFormStatus,
      isComputeOnSaveDialogOpened,
      ratingFormStatus,
      riskFormStatus,
      isDetailsFormInvalid,
      isRatingFormInvalid,
      isRiskFormInvalid,
      isClientFormInvalid: computed(
        () =>
          isFormInValid() ||
          isDetailsFormInvalid() ||
          isRatingFormInvalid() ||
          isRiskFormInvalid(),
      ),

      formRawValue: computed(() => {
        formValue();
        return toPiSimulationClientDto(
          /** Consider clientId() as Id for client, only when clientType is STORED */
          store.clientType() === ClientOptions.STORED
            ? (store.clientId() ?? 0)
            : 0,
          store.deviceId() ?? 0,
          form.getRawValue(),
          isAttachedWithKorus(),
          store.isExternal(),
        );
      }),
    };
  }),

  withMethods((store, snackBar = inject(SnackBarService)) => {
    const form = store.form();
    const router = inject(Router);
    const internalNafControl = form.controls.risks.controls.internalNAFCode;
    const officialNafControl = form.controls.risks.controls.officialNAFCode;
    const referentials = store.clientFormReferentials().data.referentials;

    const createPiClientCommand = injectPiClientCreateCommand();
    const updatePiClientCommand = injectPiClientUpdateCommand();
    const patchLocalIds = (data: PiSimulationClientDto) => {
      if (!!data.details.localIds && data.details.localIds.length > 0) {
        const uniqueLocalId = new Set();
        const sortedAliasTypeList = referentials
          .aliasType()
          .sort((x, y) => x.label.localeCompare(y.label));
        const filteredUniqueLocalId = data.details.localIds.filter((items) => {
          if (uniqueLocalId.has(items.localIdTypeId)) {
            return false;
          } else {
            uniqueLocalId.add(items.localIdTypeId);
            return true;
          }
        });
        const aliasTypeList = sortedAliasTypeList.filter((alias) =>
          filteredUniqueLocalId.find(
            (localId) => alias.code === localId.localIdTypeId,
          ),
        );
        const localIdList = aliasTypeList.map((localId) =>
          filteredUniqueLocalId.find((x) => x.localIdTypeId === localId.code),
        );
        if (typeof localIdList !== 'undefined') {
          store.localIdList.set(localIdList);
        }
      }
    };

    const reloadRouteId = async (clientId: number) => {
      const clientIdType = ClientOptions.STORED;
      await router.navigate(
        [
          'simulation',
          'pi',
          store.creditFileId(),
          store.deviceId(),
          store.fileName(),
          'client',
          clientIdType,
          clientId,
        ],
        {
          replaceUrl: true,
        },
      );
    };

    const proceedSave = async () => {
      const creditFileId = store.creditFileId();
      if (!creditFileId) return;
      try {
        const request: UpsertPiSimulationClientRequestDto = {
          creditFileId: creditFileId,
          client: store.formRawValue(),
        };

        const clientid = store.forEdit()
          ? (await updatePiClientCommand.mutateAsync(request)).data
          : (await createPiClientCommand.mutateAsync(request)).data;

        snackBar.show(PI_SIMUL_MESSAGE.client_save_successful);

        // Reload client page with client Id after creation
        if (!store.forEdit()) {
          await reloadRouteId(clientid);
          patchState(store, {
            clientId: clientid,
            clientType: ClientOptions.STORED,
          });
        }
      } catch (e) {
        void e;
        // Swallow the error
      }
    };

    const checkIfKycExist = (kycId: number): boolean => {
      return store
        .existingAttachedKycList()
        .some((k) => k === kycId.toString());
    };

    return {
      syncPiClient: rxMethod<
        SuccessResponse<PiSimulationClientDto> | undefined
      >(
        pipe(
          filter(() => !store.queryAutoFetchDisabled()),
          filter((response) => !!response),
          // delay(0),
          tap((response) => {
            const data = response.data;

            // eslint-disable-next-line @typescript-eslint/no-unnecessary-condition
            if (data !== null) {
              patchState(store, {
                deviceId: data.details.deviceId,
                clientName: data.details.shortName,
              });
              const model = fromPiSimulationClientDto(data);
              form.patchValue(model);
              form.markAsPristine();
              patchLocalIds(data);
              patchState(store, {
                isPageInitializing: false,
              });
            }
          }),
        ),
      ),

      updateFromKycSearch: rxMethod<
        SuccessResponse<PiSimulationClientDto> | undefined
      >(
        pipe(
          filter((response) => !!response),
          tap((response) => {
            const data = response.data;

            // eslint-disable-next-line @typescript-eslint/no-unnecessary-condition
            if (data) {
              const model = fromPiSimulationClientDto(data);

              const details = removeNullishProperties(model.details);
              if (data.details.korusId === null) {
                delete details.shortName;
              }

              const ratings = removeNullishProperties(model.ratings);
              const risks = removeNullishProperties(model.risks);
              const patchData = { details, ratings, risks };
              form.patchValue(patchData);

              patchLocalIds(data);
            }
          }),
        ),
      ),

      onComputeOnSaveConfirmClick: async (computeOnSave: boolean) => {
        store.isComputeOnSaveDialogOpened.set(false);
        await proceedSave();
        await store.simulationManager.onComputeDecision(computeOnSave);
      },

      clearNafCodeControls() {
        if (!store.form().pristine) {
          internalNafControl.patchValue(null);
          officialNafControl.patchValue(null);
        }
      },

      async onSaveSubmitClick() {
        // Calculation implementation: To check if current PI have calculation done, display the message
        if (store.simulationManager.attributeMap().get('hasPiDetails')) {
          store.isComputeOnSaveDialogOpened.set(true);
        } else {
          await proceedSave();
        }
      },

      checkIfKycExist,
    };
  }),

  withHooks((store) => {
    const form = store.form();
    const shortNameControl = form.controls.details.controls.shortName;
    const longNameControl = form.controls.details.controls.longName;
    return {
      onInit() {
        effect(() => {
          if (store.isReadOnly()) {
            untracked(() => {
              form.disable();
            });
          }
        });
        /** ShortName and LongName field values are populated from Unicorn and should not be editable when client is attached with Korus */
        effect(() => {
          if (store.isAttachedWithKorus()) {
            untracked(() => {
              shortNameControl.disable();
              longNameControl.disable();
            });
          } else if (!store.isReadOnly()) {
            untracked(() => {
              shortNameControl.enable();
              longNameControl.enable();
            });
          }
        });
        /** CRE1-1548 ShortName field should be optional when client attached with Korus or KYC */
        effect(() => {
          if (store.isAttachedWithKorus() || store.isUpdatedWithKyc()) {
            untracked(() => {
              updateControl(shortNameControl, {
                removeValidators: [Validators.required],
              });
            });
          } else {
            untracked(() => {
              updateControl(shortNameControl, {
                addValidators: [Validators.required],
              });
            });
          }
        });
      },
    };
  }),
);
