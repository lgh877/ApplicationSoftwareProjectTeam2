using ApplicationSoftwareProjectTeam2.entities;

namespace ApplicationSoftwareProjectTeam2
{
    public partial class GamePanel : Form
    {
        List<LivingEntity> livingentities;
        List<Entity> allentities;
        LinkedList<Entity> entities;
        int levelTickCount, worldWidth, worldHeight, currentWidth, currentHeight;
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
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            random = new CrossPlatformRandom();
            randomSeed = 0;
            levelTickCount = 0;
            livingentities = new List<LivingEntity>();
            allentities = new List<Entity>();
            entities = new LinkedList<Entity>();
            worldWidth = 1000; worldHeight = 500;
            currentWidth = this.Width - 50;
            this.currentHeight = (int)(currentWidth * 0.5);
            panelPlayScreen.Width = currentWidth; panelPlayScreen.Height = currentHeight;

            for (int i = 0; i < 100; i++)
            {
                random.setSeed(randomSeed++);
                LivingEntity test = new LivingEntity(this);
                test.setPosition(random.Next(1000) - 500, random.Next(450));
                test.team = "tets";
                addFreshLivingEntity(ref test);
            }
        }

        public void addFreshLivingEntity(ref LivingEntity? entity)
        {
            livingentities.Add(entity);
        }
        public void addFreshEntity(ref Entity entity)
        {
            entities.AddFirst(entity);
        }

        private void logicTick_Tick(object sender, EventArgs e)
        {
            int totalSize = livingentities.Count + entities.Count;
            allentities = new List<Entity>(totalSize);
            allentities.AddRange(livingentities);
            foreach (var entity in entities) allentities.Add(entity);
            if (++levelTickCount == 2)
            {
                levelTickCount = 0;
                foreach (Entity entity in allentities) entity.tick();
            }
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
                foreach (Entity entity in allentities)
                {
                    float x = Lerp(entity.xold, entity.x);
                    float z = Lerp(entity.zold, entity.z);
                    // 엔티티의 world 좌표(entity.x, entity.y)를 renderPanel 좌표로 변환
                    int screenX = panelPlayScreen.Width / 2 + (int)(x * scale * 6.184 / Math.Cbrt(z + 250));
                    int screenY = (int)(currentHeight - z * scale * 6.184 / Math.Cbrt(z + 250));

                    // 예시로 엔티티를 원으로 표현 (50% 중심 정렬)
                    int size = (int)(40 * scale * 6.184 / Math.Cbrt(z + 250)); // 엔티티 크기 (픽셀)

                    g.DrawImage(entity.Image, screenX - size / 2, screenY - size, size, size);

                    //entity.Width = size; entity.Height = size;
                    //entity.Location = new Point(screenX - size / 2, screenY - size);
                    //e.Graphics.FillEllipse(Brushes.White, screenX - (size / 2), screenY - (size / 2), size, size);
                }
            }
            buffer.Render(panelPlayScreen.CreateGraphics());
        }

        private void panelPlayScreen_Paint(object sender, PaintEventArgs e)
        {
            // 배경 클리어
            //e.Graphics.Clear(Color.Gray);
            Graphics g = e.Graphics;

            if (allentities.Count > 0)
            {
                // renderPanel의 ClientSize를 기준으로 스케일 계산
                float scale = (float)panelPlayScreen.Width / (float)worldWidth;

                // 각 엔티티를 화면에 그립니다.
                foreach (Entity entity in allentities)
                {
                    float x = 500 + Lerp(entity.xold, entity.x);
                    float z = Lerp(entity.zold, entity.z);
                    // 엔티티의 world 좌표(entity.x, entity.y)를 renderPanel 좌표로 변환
                    int screenX = (int)(x * scale * 6.184 / Math.Cbrt(z + 250));
                    int screenY = (int)(currentHeight - z * scale * 6.184 / Math.Cbrt(z + 250));

                    // 예시로 엔티티를 원으로 표현 (50% 중심 정렬)
                    int size = (int)(40 * scale * 6.184 / Math.Cbrt(z + 250)); // 엔티티 크기 (픽셀)

                    g.DrawImage(entity.Image, screenX - size / 2, screenY - size, size, size);

                    //entity.Width = size; entity.Height = size;
                    //entity.Location = new Point(screenX - size / 2, screenY - size);
                    //e.Graphics.FillEllipse(Brushes.White, screenX - (size / 2), screenY - (size / 2), size, size);
                }
            }
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

            buffer = bufferContext.Allocate(panelPlayScreen.CreateGraphics(), panelPlayScreen.DisplayRectangle);
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
