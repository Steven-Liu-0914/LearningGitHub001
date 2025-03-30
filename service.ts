import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { RequestParams, SuccessResponse } from '@crew/shared/model';
import { lastValueFrom } from 'rxjs';

import { ClientDetailsSearch } from '../model/client';
import { ClientDto, UpsertClientRequestDto } from '../model/client-cpy-dto';
import {
  CphClientDto,
  UpsertCphClientRequestDto,
} from '../model/cph/cph-client-dto';

@Injectable({
  providedIn: 'root',
})
export class CreditRequestClientService {
  readonly #http: HttpClient = inject(HttpClient);

  createClientAsync = (
    upsertPiSimulationClientRequestDto: UpsertClientRequestDto,
  ) => {
    return lastValueFrom(this.createClient(upsertPiSimulationClientRequestDto));
  };

  createClient = (upsertClientRequestDto: UpsertClientRequestDto) => {
    return this.#http.post<SuccessResponse<number>>(
      `client`,
      upsertClientRequestDto,
    );
  };
  updateClientAsync = (
    upsertPiSimulationClientRequestDto: UpsertClientRequestDto,
  ) => {
    return lastValueFrom(this.updateClient(upsertPiSimulationClientRequestDto));
  };

  updateClient = (upsertClientRequestDto: UpsertClientRequestDto) => {
    return this.#http.put<SuccessResponse<number>>(
      `client`,
      upsertClientRequestDto,
    );
  };

  /**
   * @param clientId the Id of client of the PI simulation to fetch client details
   * @param korusId the Korus Id to fetch beneficiary details
   * @param kycId the KYC ID to fetch KYC details
   * @returns a Promise delivering the PI Simulation client response
   */
  getClientAsync = (params: RequestParams<ClientDetailsSearch>) => {
    return lastValueFrom(this.getClient(params));
  };

  getClient = (params: RequestParams<ClientDetailsSearch>) => {
    return this.#http.get<SuccessResponse<ClientDto>>('client/details', {
      params: new HttpParams({ fromObject: params }),
    });
  };

  // CPH
  updateCphClientAsync = (
    upsertPiSimulationCphClientRequestDto: UpsertCphClientRequestDto,
  ) => {
    return lastValueFrom(
      this.updateCphClient(upsertPiSimulationCphClientRequestDto),
    );
  };

  updateCphClient = (upsertCphClientRequestDto: UpsertCphClientRequestDto) => {
    return this.#http.put<SuccessResponse<number>>(
      `pisimulation/client`,
      upsertCphClientRequestDto,
    );
  };

  /**
   * @param clientId the Id of client of the PI simulation to fetch CPH client details
   * @param korusId the Korus Id to fetch beneficiary details
   * @param kycId the KYC ID to fetch KYC details
   * @returns a Promise delivering the PI Simulation client response
   */
  getCphClientAsync = (params: RequestParams<ClientDetailsSearch>) => {
    return lastValueFrom(this.getCphClient(params));
  };

  getCphClient = (params: RequestParams<ClientDetailsSearch>) => {
    return this.#http.get<SuccessResponse<CphClientDto>>('client/details', {
      params: new HttpParams({ fromObject: params }),
    });
  };
}
