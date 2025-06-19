using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.resources.sounds;

namespace ApplicationSoftwareProjectTeam2.entities
{
    public class LostMessageEntity : Entity
    {
        public LostMessageEntity(GamePanel level) : base(level)
        {
            visualSize = 10f; width = 400;
            Image = Properties.Resources.lostMessage;
            team = level.clientPlayer.playerName;
            setPosition(0, 0, 0);
        }
        public override void tick()
        {
            tickCount++;
            y = level.Lerp(y, 200, 0.1f);
            if (tickCount == 30)
            {
                if (level.clientPlayer.lifeLeft == 0)
                {
                    level.playSound(SoundCache.gameOver);
                    level.addFreshEntity(new GameOverEntity(level));
                }
                else
                {
                    level.isGameRunning = false;
                    level.RecoverPlayerEntities();
                }
                discard();
            }
        }
    }
}
