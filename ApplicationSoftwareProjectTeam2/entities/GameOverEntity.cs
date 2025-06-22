using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class GameOverEntity : Entity
    {
        public GameOverEntity(GamePanel level) : base(level)
        {
            visualSize = 10f; width = 600;
            Image = Properties.Resources.gameOverMessage;
            team = level.clientPlayer.playerName;
            setPosition(0, 0, 0);
        }
        public override void tick()
        {
            tickCount++;
            y = level.Lerp(y, 350, 0.03f);
            if (tickCount == 80)
            {
                level.logicTick.Enabled = false;
            }
        }
    }
}
