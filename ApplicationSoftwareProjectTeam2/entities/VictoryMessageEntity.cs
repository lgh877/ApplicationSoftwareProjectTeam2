using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class VictoryMessageEntity : Entity
    {
        public VictoryMessageEntity(GamePanel level) : base(level)
        {
            visualSize = 10f; width = 400;
            Image = Properties.Resources.victoryMessage;
            team = level.clientPlayer.playerName;
            setPosition(0, 0, 0);
        }
        public override void tick()
        {
            tickCount++;
            y = level.Lerp(y, 200, 0.1f);
            if (tickCount == 30)
            {
                level.RecoverPlayerEntities();
                level.isGameRunning = false;
                discard();
            }
        }
    }
}
