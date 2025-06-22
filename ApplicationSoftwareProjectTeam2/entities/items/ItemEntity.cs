using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationSoftwareProjectTeam2.items;

namespace ApplicationSoftwareProjectTeam2.entities.items
{
    public class ItemEntity : LivingEntity
    {
        protected Item mainItem;
        public ItemEntity(GamePanel level) : base(level)
        {
            currentHealth = 10;
            moveSpeed = 5;
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
                hasAi = false;
            }
        }
        public override void detectLivingEntityAndMerge(Object? sender, EventArgs e)
        {
            #region 판매 부분
            if (x < -470 && z < 70)
            {
                level.modifyGold(cost / 2); // 덱에서 제거될 때 골드 반환
                level.createNumberEntity(cost / 2, (int)x, (int)y + 10, (int)z);
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
                    bool canMerge = true;
                    foreach (var equippedItem in item.EquippedItems)
                    {
                        if (equippedItem.Id == mainItem.Id)
                        {
                            canMerge = false; // 이미 같은 아이템이 장착되어 있으면 조합 불가
                            break;
                        }
                    }
                    if (!canMerge) continue;
                    grabOccurred();
                    mainItem.ApplyTo(item);
                    discard();
                    break;
                }
            }
            #endregion
        }
    }
}
