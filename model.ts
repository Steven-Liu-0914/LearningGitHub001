import { LiquidityDataDto } from './liquidity-data-dto';

export function createLiquidityData() {
  return {
    id: null as number | null,
    liqProductTypeId: null as number | null,
    isRevolvingCredit: null as boolean | null,
    overriddenStepUpInMonths: null as number | null,
    creditClaimsPledgedToCentralBanksId: null as number | null,
    isBackupLine: null as boolean | null,
    isNoticePeriod: null as boolean | null,
    overriddenProbabilityInPercentage: null as number | null,
    indexReferenceId: null as string | null,
    ratingTrigger: null as number | null,
  };
}

export type LiquidityData = ReturnType<typeof createLiquidityData>;

export function defaultLiquidityData() {
  const liquidityData = createLiquidityData();
  liquidityData.id = 0;
  liquidityData.liqProductTypeId = 4;
  liquidityData.overriddenProbabilityInPercentage = null;
  liquidityData.overriddenStepUpInMonths = null;
  liquidityData.isRevolvingCredit = false;
  liquidityData.creditClaimsPledgedToCentralBanksId = 3;
  liquidityData.isBackupLine = false;
  liquidityData.isNoticePeriod = false;
  liquidityData.indexReferenceId = 'Other';
  liquidityData.ratingTrigger = 5;
  return liquidityData;
}

export function toLiquidityDataDto(
  model: LiquidityData | null,
  mainProductId: string | null,
) {
  if (mainProductId?.startsWith('TRE') || mainProductId?.startsWith('SIG')) {
    if (model !== null) {
      const dto = {
        id: model.id,
        liq_product_type_id: model.liqProductTypeId,
        is_revolving_credit: model.isRevolvingCredit,
        overriden_time_to_step_up: model.overriddenStepUpInMonths,
        credit_claims_pledged_to_central_banks_id:
          model.creditClaimsPledgedToCentralBanksId,
        is_backup_line: model.isBackupLine,
        is_notice_period: model.isNoticePeriod,
        overriden_prepayment_probability:
          model.overriddenProbabilityInPercentage === null
            ? null
            : model.overriddenProbabilityInPercentage / 100,
        index_reference_id: model.indexReferenceId,
        rating_trigger_id: model.ratingTrigger,
      } as LiquidityDataDto;

      return Object.values(dto).every((x) => x === null) ? null : dto;
    }
  }

  return null;
}
