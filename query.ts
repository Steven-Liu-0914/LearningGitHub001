import { inject, Signal } from '@angular/core';
import { RequestParams } from '@crew/shared/model';
import {
  injectMutation,
  injectQuery,
  QueryClient,
} from '@tanstack/angular-query-experimental';

import {
  ClientDetailsSearch,
  GetClientsInFlattenHierarchyRequestParams,
} from '../model/client';
import { ClientDto } from '../model/client-cpy-dto';
import { CphClientDto } from '../model/cph/cph-client-dto';
import { CreditRequestClientService } from './credit-request-client.service';

export const injectClientCreateCommand = (deviceId: Signal<number>) => {
  const clientService = inject(CreditRequestClientService);
  const queryClient = inject(QueryClient);
  return injectMutation(() => ({
    mutationFn: (updateDto: { client: ClientDto; creditFileId: number }) =>
      clientService.createClientAsync(updateDto),
    onSuccess: async () => {
      const getClientsParam: RequestParams<GetClientsInFlattenHierarchyRequestParams> =
        {
          deviceId: deviceId(),
        };
      // Invalidate injectGetClientsInFlattenHierarchyQuery
      await queryClient.invalidateQueries({
        queryKey: ['client', 'list', getClientsParam],
      });
    },
  }));
};

export const injectClientUpdateCommand = () => {
  const clientService = inject(CreditRequestClientService);
  const queryClient = inject(QueryClient);
  return injectMutation(() => ({
    mutationKey: ['cre-client', 'update'],
    mutationFn: (updateDto: { client: ClientDto; creditFileId: number }) =>
      clientService.updateClientAsync(updateDto),
    onSuccess: async (response) => {
      const searchClientParam: RequestParams<ClientDetailsSearch> = {
        clientId: response.data,
      };
      await queryClient.invalidateQueries({
        queryKey: ['credit-request-client', searchClientParam],
      });
    },
  }));
};

export const injectCphClientUpdateCommand = () => {
  const clientService = inject(CreditRequestClientService);
  const queryClient = inject(QueryClient);
  return injectMutation(() => ({
    mutationFn: (updateDto: { client: CphClientDto; simulationId: number }) =>
      clientService.updateCphClientAsync(updateDto),
    onSuccess: async (response) => {
      const searchClientParam: RequestParams<ClientDetailsSearch> = {
        clientId: response.data,
      };
      await queryClient.invalidateQueries({
        queryKey: ['credit-request-client', searchClientParam],
      });
    },
  }));
};

export const injectClientGetQuery = (
  searchParams: Signal<RequestParams<ClientDetailsSearch>>,
  isReferentialsPending: Signal<boolean>,
) => {
  const clientService = inject(CreditRequestClientService);
  const response = injectQuery(() => {
    return {
      enabled:
        !!searchParams().clientId &&
        !isReferentialsPending() &&
        !!searchParams().clientType,
      queryKey: ['credit-request-client', searchParams()],
      queryFn: () => clientService.getClientAsync(searchParams()),
    };
  });

  return response;
};

export const injectCphClientGetQuery = (
  searchParams: Signal<RequestParams<ClientDetailsSearch>>,
  isReferentialsPending: Signal<boolean>,
) => {
  const clientService = inject(CreditRequestClientService);
  const response = injectQuery(() => {
    return {
      enabled: !!searchParams().clientId && !isReferentialsPending(),
      queryKey: ['credit-request-cph-client', searchParams()],
      queryFn: () => clientService.getCphClientAsync(searchParams()),
    };
  });

  return response;
};
