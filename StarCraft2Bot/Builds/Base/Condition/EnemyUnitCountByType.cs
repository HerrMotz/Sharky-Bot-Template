using Sharky;

namespace StarCraft2Bot.Builds.Base.Condition
{
    public class EnemyUnitCountByType : ICondition
    {
        public EnemyUnitCountByType(UnitTypes unitType, int unitCount, UnitCountService unitCountService)
        {
            UnitType = unitType;
            UnitCountService = unitCountService;
            UnitCount = unitCount;
        }

        public UnitTypes UnitType { get; set; }
        
        public int UnitCount { get; set; }

        public UnitCountService UnitCountService { get; set; }

        public bool IsFulfilled()
        {
            return UnitCountService.EquivalentEnemyTypeCount(UnitType) > UnitCount;
        }
    }
}
