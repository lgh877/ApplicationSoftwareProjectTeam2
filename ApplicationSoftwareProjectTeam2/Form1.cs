using ApplicationSoftwareProjectTeam2.entities;

namespace ApplicationSoftwareProjectTeam2
{
    public partial class Form1 : Form
    {
        List<LivingEntity> livingentities;
        LinkedList<Entity> entities;
        int levelTickCount;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            levelTickCount = 0;
            livingentities = new List<LivingEntity>();
            entities = new LinkedList<Entity>();
        }

        public void addFreshLivingEntity(LivingEntity entity) { livingentities.Add(entity); }
        public void addFreshEntity(Entity entity) { entities.AddFirst(entity); }

        private void logicTick_Tick(object sender, EventArgs e)
        {
            int totalSize = livingentities.Count + entities.Count;
            List<Entity> allentities = new List<Entity>(totalSize);
            allentities.AddRange(livingentities);
            foreach (var entity in entities) allentities.Add(entity);
            if (levelTickCount++ == 3)
            {
                levelTickCount = 0;
                foreach (Entity entity in allentities) entity.tick();
            }
        }
    }
}
