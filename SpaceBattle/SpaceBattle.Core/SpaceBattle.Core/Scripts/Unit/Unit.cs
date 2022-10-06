using CosmosEngine;
using System.Drawing;

namespace SpaceBattle
{
    public class Unit : GameBehaviour
    {
        private int maxHealth;
        private int maxShield;
        private int teamID;
        private int regenCooldown;
        private float currentHealth;
        private float regenHealth;
        private float regenShield;
        private float damage;
        private float currentShield;
        private float speed;
        private string name;
        private string description;
        private bool isAlive;
        private UnitType type;
        private Sprite graphics;
        private Unit target;

        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public int MaxShield { get => maxShield; set => maxShield = value; }
        public int TeamID { get { return teamID; } set { teamID = value; } }
        public int RegenCooldown { get => regenCooldown; set => regenCooldown = value; }
        public float CurrentHealth { get => currentHealth; set => currentHealth = value; }
        public float RegenHealth { get => regenHealth; set => regenHealth = value; }
        public float RegenShield { get => regenShield; set => regenShield = value; }
        public float Damage { get => damage; set => damage = value; }
        public float CurrentShield { get => currentShield; set => currentShield = value; }
        public float Speed { get => speed; set => speed = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public bool IsAlive { get => isAlive; set => isAlive = value; }
        public UnitType Type { get => type; set => type = value; }
        public Sprite Graphics { get => graphics; set => graphics = value; }
        public Unit Target { get => target; set => target = value; }

        public Unit(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        protected override void Start()
        {
            currentHealth = maxHealth;
            currentShield = MaxShield;
            isAlive = true;
        }

        protected override void Update()
        {
            Debug.QuickLog(Transform.Position);
            Debug.Log(InputManager.MousePosition);
            Transform.RotateTowards(Camera.Main.ScreenToWorld(InputManager.MousePosition), 180f);
            if(TeamID != 1)
            {
                Graphics = ArtContent.InterceptorEnemy;
            }
            else
            {
                Graphics = ArtContent.Interceptor;
            }
        }
        
        public void TakeDamage(float incDamage)
        {
            currentHealth -= incDamage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
            }
        }

        public static Unit Interceptor
        {
            get
            {
                Unit Interceptor = new Unit("Interceptor", "Fast and agile unit with low firepower but great evasive abilities.")
                {
                    MaxHealth = 50,
                    MaxShield = 92,
                    TeamID = 1, // Set as # in player list
                    Speed = 2.7f,
                    Damage = 8f,
                    RegenCooldown = 500, // 5 seconds
                    RegenHealth = 0.2f,
                    RegenShield = 0.3f,
                    Graphics = ArtContent.Interceptor
                };
            return Interceptor;
            }
        }

        

        public override string ToString()
        {
            return $"Unit [{Type}] - {CurrentHealth:F0}/{MaxHealth:F0} - IsAlive:{IsAlive}";
        }

    }
}
