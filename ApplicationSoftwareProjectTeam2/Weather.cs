using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationSoftwareProjectTeam2
{
    public enum MainWeathers
    {
        Clear,
        Fog,
        Hot,
        Cold
    }

    public enum SubWeathers
    {
        Clear,
        S_Fog,
        S_Hot,
        S_Cold
    }

    class Weather
    {
        MainWeathers curMainWeather;
        SubWeathers curSubWeather;

        public Weather()
        {
            InitialWeather(curMainWeather, curSubWeather);
        }

        private void InitialWeather(MainWeathers cmw, SubWeathers csw)
        {
            cmw = MainWeathers.Clear;
            csw = SubWeathers.Clear;
            // 초기 날씨를 어떻게 할 지 상의 필요, 일단은 맑음으로 설정했으나 추후 랜덤으로 바꾼다거나 할 수 있음
        }

        // 날씨 효과를 실제로 적용하는 함수, 각 전투가 시작할 때 호출해야 함
        // 일부 비전투 상태에도 영향을 주는 효과도 있는데 그건 나중에 고려
        public void EffectWeather()
        {
            
        }
    }
}
