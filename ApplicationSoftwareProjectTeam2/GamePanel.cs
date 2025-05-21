using ApplicationSoftwareProjectTeam2.entities;
using System.Xml.Linq;

namespace ApplicationSoftwareProjectTeam2
{
    public partial class GamePanel : Form
    {
        //캐릭터 객체들을 관리하는 리스트로, 업데이트가 비교적 드물어 그냥 리스트로 선언함
        public List<LivingEntity?> livingentities = new List<LivingEntity?> ();
        //투사체, 기술 등에 쓰이는 객체들을 관리하는 리스트로, 리스트에 삽입 / 삭제가 자주 발생하므로 링크드 리스트 사용
        public LinkedList<Entity?> entities = new LinkedList<Entity?> ();
        //객체 업데이트 및 렌더링에 사용되는 리스트
        List<Entity?> allentities = new List<Entity?> ();
        int levelTickCount, currentWidth, currentHeight;
        const int worldWidth = 1000, worldHeight = 500;
        public CrossPlatformRandom random;
        public ulong randomSeed;
        private BufferedGraphicsContext bufferContext;
        private BufferedGraphics buffer;



        public GamePanel()
        {
            InitializeComponent();
            bufferContext = BufferedGraphicsManager.Current;
            buffer = bufferContext.Allocate(panelPlayScreen.CreateGraphics(), panelPlayScreen.DisplayRectangle);
            this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //bufferContext = BufferedGraphicsManager.Current;
            //buffer = bufferContext.Allocate(panelPlayScreen.CreateGraphics(), panelPlayScreen.DisplayRectangle);
            //this.DoubleBuffered = true;
            random = new CrossPlatformRandom();
            randomSeed = 0;
            levelTickCount = 0;
            //livingentities = new List<LivingEntity?>();
            //allentities = new List<Entity?>();
            //entities = new LinkedList<Entity?>();

            this.Width += 1;

            for (int i = 0; i < 200; i++)
            {
                LivingEntity test = new LivingEntity(this);
                test.setPosition(getRandomInteger(1000) - 500, getRandomInteger(450));
                test.team = getRandomInteger(101).ToString();
                addFreshLivingEntity(test);
            }

            currentWidth = this.Width - 50;
            this.currentHeight = (int)(currentWidth * 0.5);
            panelPlayScreen.Width = currentWidth; panelPlayScreen.Height = currentHeight;
        }

        public void addFreshLivingEntity(LivingEntity? entity)
        {
            livingentities.Add(entity);
        }
        public void addFreshEntity(Entity? entity)
        {
            entities.AddFirst(entity);
        }
        private void logicTick_Tick(object sender, EventArgs e)
        {
            if (++levelTickCount == 2)
            {
                levelTickCount = 0;
                for (int i = livingentities.Count - 1; i != -1; i--)
                {
                    livingentities[i].tick();
                    if (livingentities[i].shouldRemove)
                    {
                        livingentities[i] = null;
                        livingentities.RemoveAt(i);
                    }
                }
                var node = entities.First;
                while (node != null)
                {
                    var nextNode2 = node.Next;
                    node.Value.tick();

                    if (node.Value.shouldRemove)
                    {
                        node.Value = null;
                        entities.Remove(node);
                    }

                    node = nextNode2;
                }
            }
            allentities.Clear();
            allentities.Capacity = livingentities.Count + entities.Count;
            allentities.AddRange(livingentities);
            foreach (var entity in entities) allentities.Add(entity);
            allentities.Sort((a, b) => a.z.CompareTo(b.z));
            renderEntities();
        }

        private void renderEntities()
        {
            Graphics g = buffer.Graphics;
            g.Clear(panelPlayScreen.BackColor);

            if (allentities.Count > 0)
            {
                // renderPanel의 ClientSize를 기준으로 스케일 계산
                float scale = (float)panelPlayScreen.Width / (float)worldWidth;

                // 각 엔티티를 화면에 그립니다.
                for (int i = allentities.Count - 1; i != -1; i--)
                {
                    float x = Lerp(allentities[i].xold, allentities[i].x);
                    float z = Lerp(allentities[i].zold, allentities[i].z);
                    double scale2 = 6.184 / Math.Cbrt(z + 250);
                    // 엔티티의 world 좌표(entity.x, entity.y)를 renderPanel 좌표로 변환
                    int screenX = panelPlayScreen.Width / 2 + (int)(x * scale * scale2);
                    int screenY = (int)(currentHeight - z * scale * scale2);

                    // 예시로 엔티티를 원으로 표현 (50% 중심 정렬)
                    int size = (int)(allentities[i].visualSize * scale * scale2); // 엔티티 크기 (픽셀)

                    g.DrawImage(allentities[i].Image, screenX - size / 2, screenY - size, size, size);

                    //entity.Width = size; entity.Height = size;
                    //entity.Location = new Point(screenX - size / 2, screenY - size);
                    //e.Graphics.FillEllipse(Brushes.White, screenX - (size / 2), screenY - (size / 2), size, size);
                }
            }
            buffer.Render(panelPlayScreen.CreateGraphics());
        }

        public int getRandomInteger(int max)
        {
            random.setSeed(randomSeed++);
            return random.Next(max);
        }

        public float Lerp(float f1, float f2)
        {
            return f1 + (f2 - f1) * (float)levelTickCount * 0.5f;
        }

        private void GamePanel_Resize(object sender, EventArgs e)
        {
            currentWidth = this.Width - 50;
            this.currentHeight = (int)(currentWidth * 0.5);
            panelPlayScreen.Width = currentWidth; panelPlayScreen.Height = currentHeight;
            AllocateBuffer(); // 크기 변경 시 버퍼 재할당
            renderEntities(); // 화면 다시 그리기
        }
        private void AllocateBuffer()
        {
            if (buffer != null)
            {
                buffer.Dispose(); // 기존 버퍼 제거
            }

            bufferContext = BufferedGraphicsManager.Current;
            buffer = bufferContext.Allocate(panelPlayScreen.CreateGraphics(), panelPlayScreen.DisplayRectangle);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logicTick.Enabled = !logicTick.Enabled;
        }



        public List<T> getAllLivingEntities<T>() where T : LivingEntity
        {
            List<T> result = new List<T>(livingentities.Count);
            foreach (var entity in livingentities)
            {
                if (entity is T)
                {
                    result.Add(entity as T);
                }
            }
            return result;
        }

        public List<T> getAllEntities<T>() where T : Entity
        {
            List<T> result = new List<T>(entities.Count);
            foreach (var entity in entities)
            {
                if (entity is T)
                {
                    result.Add(entity as T);
                }
            }
            return result;
        }
    }
}
