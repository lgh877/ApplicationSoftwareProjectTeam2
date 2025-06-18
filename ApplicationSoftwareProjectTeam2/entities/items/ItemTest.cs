using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.entities.projectiles;
using ApplicationSoftwareProjectTeam2.items;
using ApplicationSoftwareProjectTeam2.utils;

namespace ApplicationSoftwareProjectTeam2.entities.creatures
{
    public class ItemTest : LivingEntity
    {
        public string Name;

        public static List<Image> images = new List<Image>()
        {
            Properties.Resources.ItemTest
        };
        public ItemTest(GamePanel level) : base(level)
        {
            cost = 2;
            visualSize = 1f; width = 40; height = 70; weight = 10; pushPower = 0;
            Image = images[0];
            direction = Direction.Right;
            maxHealth = 100; currentHealth = 100;
            attackDamage = 0;
            moveSpeed = 5;
            renderType = 0;

            Name = "TestItem";
        }
        public override void scaleEntity(float scale)
        {
            base.scaleEntity(scale);
            maxHealth *= scale; currentHealth = maxHealth;
            attackDamage *= scale; pushPower = (int)(pushPower * scale); moveSpeed = (int)(moveSpeed * Math.Sqrt(scale));
        }
        public override EntityTypes getEntityType()
        {
            return EntityTypes.Items;
        }
        public override void releaseFromMouse()
        {
            direction = Direction.Right;
            //마우스에서 놓았을 때 z값이 200보다 낮다면 해당 객체를 level의 livingentities에서 entities 리스트로 옮기고 hasAi를 fasle로 해주세요
                if (z > 175) setPosition(x, 0, 175);
                if (!findClosestDeckPosition()) return;
            if (hasAi)
            {
                landedEvent += detectLivingEntityAndMerge;
                level.addFreshEntity(this);
                level.livingentities.Remove(this);
                level.clientPlayer.entitiesofplayer.Remove(this); // 플레이어의 엔티티 목록에서 제거
                hasAi = false;
            }
        }

        public override void detectLivingEntityAndMerge(Object? sender, EventArgs e)
        {
            #region 판매 부분
            if (x < -470 && z < 70)
            {
                level.clientPlayer.Gold += cost / 2; // 덱에서 제거될 때 골드 반환
                level.label1.Text = $"Gold: {level.clientPlayer.Gold}";
                level.valueTupleList[deckIndex] = level.valueTupleList[deckIndex] with { Item3 = false };
                level.occupiedIndexCount--;
                deckIndex = -1; // 인덱스 초기화
                level.grabbed = false; // 덱에서 제거되었으므로 grabbed 상태 해제
                discard(); // 엔티티 제거 플래그 설정
                return; // 덱에서 제거되었으므로 더 이상 처리하지 않음
            }
            #endregion
            #region 조합 부분
            foreach (var item in level.getAllEntities<LivingEntity>())
            {
                if (!item.Equals(this) && item.getEntityType() != EntityTypes.Items && item.EquippedItems.Count < 3
                    && 40 > Math.Abs(item.x - x) + Math.Abs(item.z - z))
                {
                    grabOccurred();
                    Item data;
                    item.EquippedItems.Add(data = new Item(Name, cost, ItemType.Universal, Name));
                    discard();
                    break;
                }
            }
            ;
            #endregion
        }
        #region 캐릭터 아이디 기록
        public override byte getLivingEntityId()
        {
            return 1;
        }
        #endregion

        //********아이템이라서 모션 및 사망 처리 필요 없음

        /*public override void tickAlive()
        {
            base.tickAlive();
            switch (entityState)
            {
                case 0:
                    if (canStartTask())
                    {
                        #region 평상시에 아무렇게 걸어다니기 + 타겟 탐색
                        if (level.getRandomInteger(10) == 0)
                        {
                            if (getTarget() == null)
                            {
                                if (hadTarget)
                                {
                                    hadTarget = false;
                                }
                                LivingEntity foundTarget = detectTargetManhattan(1000);
                                if (foundTarget != null)
                                {
                                    target = foundTarget;
                                    hadTarget = true;
                                }
                            }
                            isMoving = !isMoving || getTarget() != null;
                        }
                        #endregion
                        if (isMoving)
                        {
                            if (tickCount % 4 == 0)
                            {
                                mana++;
                                if (getTarget() == null)
                                {
                                    int dir = (int)direction;
                                    dir = dir + (level.getRandomInteger(2) == 0 ? 1 : -1);
                                    dir = dir > 9 ? 0 : dir < 0 ? 9 : dir;
                                    direction = (Direction)dir;
                                }
                                else
                                {
                                    float ydiff = target.y - y;

                                    //공격 범위 안에 들어왔는지 확인
                                    if (mana < 20)
                                    {
                                        if ((target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 0.25 * (width * width + (target.width + width) * (target.width + width))
                                        && ydiff < height
                                        && ydiff > -target.height)
                                        {
                                            entityState = 1;
                                            walkTicks = 0;
                                        }
                                    }
                                    else
                                    {
                                        mana = 0;
                                        entityState = 2;
                                        walkTicks = 0;
                                    }

                                    //공격 시도가 실패한 경우 상대 방향으로 이동
                                    if (entityState == 0)
                                    {
                                        int dir = (int)direction;
                                        Vector3 targetVec = Vector3.Normalize(new Vector3(target.x - x, 0, target.z - z));

                                        if (targetVec.X > 0)
                                        {
                                            if (targetVec.Z > 0.9659) dir = 0;
                                            else if (targetVec.Z > 0.7071) dir = 1;
                                            else if (targetVec.Z > -0.7071) dir = 2;
                                            else if (targetVec.Z > -0.9659) dir = 3;
                                            else dir = 4;
                                        }
                                        else
                                        {
                                            if (targetVec.Z > 0.9659) dir = 9;
                                            else if (targetVec.Z > 0.7071) dir = 8;
                                            else if (targetVec.Z > -0.7071) dir = 7;
                                            else if (targetVec.Z > -0.9659) dir = 6;
                                            else dir = 5;
                                        }
                                        direction = (Direction)dir;
                                    }
                                }
                            }

                            if (isOnGround()) move(moveSpeed);
                        }
                    }
                    #region 캐릭터가 실제로 움직이고 있는 지 결정하는 부분으로, 애니매이션 재생에 쓰임
                    if (deltaMovement.X * deltaMovement.X + deltaMovement.Z * deltaMovement.Z > 4)
                    {
                        walkTicks++;
                        isActuallyMoving = true;

                        //걷는 애니메이션
                        if (walkTicks == 2) Image = (int)direction < 5 ? images[4] : images[6];
                        else if (walkTicks == 4) Image = (int)direction < 5 ? images[5] : images[7];
                        else if (walkTicks == 6) Image = (int)direction < 5 ? images[4] : images[6];
                        else if (walkTicks > 7) { Image = (int)direction < 5 ? images[0] : images[2]; walkTicks = 0; }
                    }
                    #endregion
                    #region 평상시 애니매이션 재생 부분
                    else
                    {
                        if (isActuallyMoving)
                        {
                            isActuallyMoving = false;
                            Image = (int)direction < 5 ? images[0] : images[2];
                        }
                        walkTicks = tickCount % 16;
                        if (walkTicks == 0) Image = (int)direction < 5 ? images[0] : images[2];
                        else if (walkTicks == 8) Image = (int)direction < 5 ? images[1] : images[3];
                    }
                    #endregion
                    break;
                case 1:
                    direction = target.x - x > 0 ? Direction.Right : Direction.Left;
                    walkTicks++;
                    switch (walkTicks)
                    {
                        case 1:
                            Image = (int)direction < 5 ? images[10] : images[16];
                            break;
                        case 3:
                            Image = (int)direction < 5 ? images[11] : images[17];
                            break;
                        case 5:
                            Image = (int)direction < 5 ? images[12] : images[18];
                            break;
                        case 7:
                            Image = (int)direction < 5 ? images[13] : images[19];
                            if (target != null && (target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 0.25 * (width * width + (target.width + width) * (target.width + width))
                                && target.y - y < height && target.y - y > -target.height)
                            {
                                doHurtTarget(target);
                            }
                            break;
                        case 9:
                            Image = (int)direction < 5 ? images[14] : images[20];
                            break;
                        case 11:
                            Image = (int)direction < 5 ? images[15] : images[21];
                            break;
                        case 13:
                            walkTicks = 0;
                            entityState = 0;
                            break;
                    }
                    break;
                case 2:
                    direction = target.x - x > 0 ? Direction.Right : Direction.Left;
                    walkTicks++;
                    switch (walkTicks)
                    {
                        case 1:
                            Image = (int)direction < 5 ? images[0] : images[2];
                            break;
                        case 3:
                            Image = (int)direction < 5 ? images[15] : images[21];
                            break;
                        case 5:
                            Image = (int)direction < 5 ? images[10] : images[16];
                            break;
                        case 7:
                            Image = (int)direction < 5 ? images[11] : images[17];
                            float distance = (float)Math.Cbrt((target.x - x) * (target.x - x) + (target.z - z) * (target.z - z));
                            Vector3 targetVec = Vector3.Normalize(new Vector3(target.x - x, 0, target.z - z));
                            int jp = (int)Math.Min(moveSpeed * 100, distance * 1.5);
                            push(targetVec.X * jp, moveSpeed * 10, targetVec.Z * jp); // 위로 점프
                            break;
                        case 9:
                            Image = (int)direction < 5 ? images[12] : images[18];
                            push(0, -moveSpeed * 12, 0);
                            break;
                        case 11:
                            Image = (int)direction < 5 ? images[13] : images[19];
                            if (target != null && (target.x - x) * (target.x - x) + (target.z - z) * (target.z - z) < 0.5 * (width * width + (target.width + width) * (target.width + width))
                                && target.y - y < height && target.y - y > -target.height)
                            {
                                doHurtTarget(target, attackDamage * 1.5f, pushPower * 3);
                            }
                            break;
                        case 13:
                            Image = (int)direction < 5 ? images[14] : images[20];
                            break;
                        case 15:
                            Image = (int)direction < 5 ? images[15] : images[21];
                            break;
                        case 17:
                            walkTicks = 0;
                            entityState = 0;
                            break;
                    }
                    break;
            }
        }
        public override void setDeath(object? sender, EventArgs e)
        {
            width *= 2; height /= 2; Image = (int)direction < 5 ? images[8] : images[9];
        }*/
    }
}
