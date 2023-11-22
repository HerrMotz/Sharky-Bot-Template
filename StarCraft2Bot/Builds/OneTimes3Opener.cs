using SC2APIProtocol;
using Sharky;
using Sharky.DefaultBot;
using Sharky.MicroControllers;
using Sharky.MicroTasks;
using StarCraft2Bot.Bot;
using Sharky.MicroTasks.Attack;
using Sharky.Proxy;
using StarCraft2Bot.Builds.Base;
using StarCraft2Bot.Builds.Base.Condition;
using StarCraft2Bot.Builds.Base.Desires;

namespace StarCraft2Bot.Builds
{
    public class OneTimes3Opener : Build
    {
        private Queue<BuildAction>? BuildOrder { get; set; }

        public OneTimes3Opener(BaseBot defaultSharkyBot) : base(defaultSharkyBot)
        {
            defaultSharkyBot.MicroController = new AdvancedMicroController(defaultSharkyBot);
            var advancedAttackTask = new AdvancedAttackTask(defaultSharkyBot, new EnemyCleanupService(defaultSharkyBot.MicroController, defaultSharkyBot.DamageService), new List<UnitTypes> { UnitTypes.TERRAN_MARINE }, 100f, true);
            defaultSharkyBot.MicroTaskData[typeof(AttackTask).Name] = advancedAttackTask;
        }

        public override void StartBuild(int frame)
        {
            base.StartBuild(frame);

            BuildOptions.StrictGasCount = true;
            BuildOptions.StrictSupplyCount = true;
            BuildOptions.StrictWorkerCount = false;

            // MacroData.DesiredUnitCounts[UnitTypes.TERRAN_SCV] = 19;

            AddAction(new BuildAction(new UnitCompletedCountCondition(UnitTypes.TERRAN_SUPPLYDEPOT, 1, UnitCountService),
                          new CustomDesire(() => {
                              MicroTaskData[typeof(WorkerScoutTask).Name].Enable();
                              // MicroTaskData[typeof(AttackTask).Name].Enable();
                          })));

            BuildOrder = new Queue<BuildAction>();

            // =========================================================================================================
            // Opening
            BuildOrder.Enqueue(new BuildAction(new SupplyCondition(14, MacroData),
                                               new SupplyDepotDesire(1, MacroData)));

            BuildOrder.Enqueue(new BuildAction(new SupplyCondition(15, MacroData),
                                               new GasBuildingCountDesire(1, MacroData)));
            
            BuildOrder.Enqueue(new BuildAction(new SupplyCondition(16, MacroData),
                new ProductionStructureDesire(UnitTypes.TERRAN_BARRACKS, 1, MacroData)));

            
            // TODO: add scouting desire
            // =========================================================================================================
            
            // =========================================================================================================
            // Proxy erkennen
            
            // ENTWEDER: Gegner hat Baracke in der Base => CC bauen, ODER: Gegner hat keine Baracke in der Base => Gegner spielt Proxy => Transition
            // Wir tun mal so als spielt der Gegner nie Proxy, TODO: anderen Fall implementieren (Hausaufgaben yay)
            //  Siehe *OnFrame*-Funktion weiter unten
            
            // Wir bauen einen Reaper und upgraden das Command Center zum Orbital Command
            BuildOrder.Enqueue(new BuildAction(new SupplyCondition(19, MacroData), 
                new UnitDesire(UnitTypes.TERRAN_REAPER, 1, MacroData.DesiredUnitCounts),
                new MorphDesire(UnitTypes.TERRAN_ORBITALCOMMAND, 1, MacroData)));
            
            BuildOrder.Enqueue(new BuildAction(new SupplyCondition(20, MacroData),
                new ProductionStructureDesire(UnitTypes.TERRAN_COMMANDCENTER, 2, MacroData)));
            
            // =========================================================================================================

            // Mit einem Orbital Command kann man Mules bauen, die irgendwie effizienter sind als SCVs
            // Man sollte die ersten beiden CCs zu OCs umbauen (lt. Johannes)
            
            // =========================================================================================================
            // Bau eines Cyclones (Factory bauen)
            
            // BuildOrder.Enqueue(new BuildAction(new UnitCountCondition(UnitTypes.TERRAN_COMMANDCENTER, 2, UnitCountService),
            //                                     new UnitDesire(UnitTypes.TERRAN_SCV, 24, MacroData.DesiredUnitCounts)));

            
            // Factory unbedingt vor dem zweiten Supply Depot bauen (warum auch immer)
            BuildOrder.Enqueue(new BuildAction(new UnitCountCondition(UnitTypes.TERRAN_COMMANDCENTER, 2, UnitCountService),
                                               new ProductionStructureDesire(UnitTypes.TERRAN_FACTORY, 1, MacroData)));

            
            // Wir bauen einen Barack Reactor, nachdem wir einen Reaper gebaut haben.
            BuildOrder.Enqueue(new BuildAction(new UnitCompletedCountCondition(UnitTypes.TERRAN_REAPER, 1, UnitCountService),
                                               new AddonStructureDesire(UnitTypes.TERRAN_BARRACKSREACTOR, 1, MacroData)));

            
            BuildOrder.Enqueue(new BuildAction(new UnitCountCondition(UnitTypes.TERRAN_FACTORY, 1, UnitCountService),
                                               new SupplyDepotDesire(2, MacroData), // TODO: vielleicht an die Stelle nach dem Cyclone verschieben
                                               new AddonStructureDesire(UnitTypes.TERRAN_FACTORYTECHLAB, 1, MacroData)));

            // Wenn das Supply Depot fertig ist, Gas ausbauen
            BuildOrder.Enqueue(new BuildAction(new SupplyCondition(22, MacroData),
                                                new GasBuildingCountDesire(2, MacroData)));

            BuildOrder.Enqueue(new BuildAction(new UnitCompletedCountCondition(UnitTypes.TERRAN_FACTORY, 1, UnitCountService),
                                               new UnitDesire(UnitTypes.TERRAN_CYCLONE, 1, MacroData.DesiredUnitCounts)));
            
            // =========================================================================================================
            
            
            // =========================================================================================================
            // Mögliche Pressuring Phase 

            // Wenn der Gegner pressured, dann soll (aber nur wenn der Cyclone gebaut wurde) dann prüfen:

            // Unter welchen Bedingungen pressured ein Gegner bzw. ist gefährlich?
            //  - man kann die Ressourcen, die die gegnerische Armee (die man sieht)
            //     gekostet hat ausrechnen und dies als Gefährlichkeit der Armee ansehen
            //  - Pro Gegner ist es wichtig, wie nah er an unserer Basis ist, und wie viele Gegner es sind.
            //      Darauf müssen wir dann entsprechend reagieren (parametrisieren und lernen, was optimal ist)

            
            // Wenn er gefährlich ist / pressured
            //  - ziehen wir alle Einheiten zurück
            //  -> wenn ein Reaper in der gegn. Armee ist
            //     - Marines
            //     - Guard Tower
            // TODO: Wie erkennen wir, dass der Gegner nicht mehr pressured?
            
            //  -> sonst
            //     - verstecken bis Cyclone fertig, weil er nicht reinkommt, weil wir uns gewallt haben
            
            // =========================================================================================================
            

            // =========================================================================================================
            // 4 Marines bauen und zweites OC
            BuildOrder.Enqueue(new BuildAction(new UnitCompletedCountCondition(UnitTypes.TERRAN_BARRACKSREACTOR, 1, UnitCountService),
                                               new UnitDesire(UnitTypes.TERRAN_MARINE, 2, MacroData.DesiredUnitCounts)));

            BuildOrder.Enqueue(new BuildAction(new WorkerCountCondition(23, UnitCountService),
                                               new MorphDesire(UnitTypes.TERRAN_ORBITALCOMMAND, 2, MacroData)));

            BuildOrder.Enqueue(new BuildAction(new UnitCompletedCountCondition(UnitTypes.TERRAN_MARINE, 2, UnitCountService),
                                               new UnitDesire(UnitTypes.TERRAN_MARINE, 4, MacroData.DesiredUnitCounts)));
            // =========================================================================================================

            // =========================================================================================================
            // Starport und Reaper Scout
            BuildOrder.Enqueue(new BuildAction(new SupplyCondition(29, MacroData),
                                               new ProductionStructureDesire(UnitTypes.TERRAN_STARPORT, 1, MacroData)));

            BuildOrder.Enqueue(new BuildAction(new UnitCountCondition(UnitTypes.TERRAN_STARPORT, 1, UnitCountService),
                                               new UnitDesire(UnitTypes.TERRAN_CYCLONE, 2, MacroData.DesiredUnitCounts)));
            // =========================================================================================================
        }

        public void ReactOnProxy()
        {
            if (BuildOrder is not null)
            {
                BuildOrder.Enqueue(new BuildAction(new SupplyCondition(1, MacroData),
                                                new ProductionStructureDesire(UnitTypes.TERRAN_BUNKER, 1, MacroData)));           
                
                // TODO: Hier muss eine Transition zu einem anderen Build angestoßen werden.
            }
        }

        public override void OnFrame(ResponseObservation observation)
        {
            base.OnFrame(observation);
            if (BuildOrder == null)
            {
                throw new InvalidOperationException("BuildOrder has not been initialized.");
            }

            if (BuildOrder.Count == 0)
            {
                return;
            }

            var nextAction = BuildOrder.Peek();
            
            // TODO: Erkennen, ob der Gegner eine Barracke gebaut hat.
            // if (gegner hat keine baracke in seiner basis)
            // {
            // BuildOrder.Clear();
            // ReactOnProxy();
            // }

            if (nextAction.AreConditionsFulfilled())
            {
                nextAction.EnforceDesires();
                BuildOrder.Dequeue();
            }
            
            // man kann die Buildorder leeren
            //  dann geht er in die nächste Transition
            
            // TODO: Wie können wir steuern, welche Transition er machen soll?
        }

        public override bool Transition(int frame)
        {
            if (BuildOrder == null)
            {
                throw new InvalidOperationException("BuildOrder has not been initialized.");
            }

            if (BuildOrder.Count == 0)
            {
                AttackData.UseAttackDataManager = true;
                return true;
            }

            return base.Transition(frame);
        }
    }
}
