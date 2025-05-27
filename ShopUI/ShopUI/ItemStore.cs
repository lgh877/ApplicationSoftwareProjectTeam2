using System.Collections.Generic;

namespace ApplicationSoftwareProjectTeam2.items
{
    public class ItemStore
    {
        public List<Item> AvailableItems { get; set; } = new List<Item>();

        public ItemStore()
        {
            LoadItems();
        }

        private void LoadItems()
        {
            //체력, 공격력, 방어력, 넉백파워, 무게(넉백 덜받게), 이동속도, 탄성력(착지/충돌에 의한 데미지발생)
            AvailableItems.Add(new Item("철갑 피부", 5, ItemType.Tank, "방어력 +n, 받는 피해 -n%"));
            AvailableItems.Add(new Item("전방 방패", 6, ItemType.Tank, "전방 피해 감소"));
            AvailableItems.Add(new Item("역반사 장치", 6, ItemType.Tank, "피해 반사"));
            AvailableItems.Add(new Item("마력 방해장", 6, ItemType.Tank, "주변 적 스킬속도 감소"));
            AvailableItems.Add(new Item("강화 심장", 5, ItemType.Universal, "최대 체력 +n, 재생속도 증가"));
            AvailableItems.Add(new Item("관통 화살", 5, ItemType.Ranged, "방어력 무시 +n%"));
            AvailableItems.Add(new Item("속사 장치", 6, ItemType.Ranged, "공격속도 +n%"));
            AvailableItems.Add(new Item("명중 향상 모듈", 5, ItemType.Ranged, "명중률 및 치명타 확률 증가"));
            AvailableItems.Add(new Item("연소 탄환", 6, ItemType.Ranged, "화염 지속 데미지"));
            AvailableItems.Add(new Item("혼란 광선기", 6, ItemType.Special, "적 명중률/스킬확률 감소"));
            AvailableItems.Add(new Item("에너지 확산기", 5, ItemType.Special, "범위 증가, 쿨타임 감소"));
            AvailableItems.Add(new Item("사이오닉 점멸기", 7, ItemType.Special, "체력 낮을 때 자동 회피"));
            AvailableItems.Add(new Item("다중 연계장치", 6, ItemType.Special, "스킬 효과 인접 아군 공유"));
        }
    }
}