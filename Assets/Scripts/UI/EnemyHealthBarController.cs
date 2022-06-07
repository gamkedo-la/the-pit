using UnityEngine.UIElements;
using Variables;

namespace UI
{
    public class EnemyHealthBarController
    {
        private readonly ProgressBar progressBar;
        private readonly HealthVariable healthVariable;
        
        public EnemyHealthBarController(VisualElement root, HealthVariable healthVariable)
        {
            this.healthVariable = healthVariable;
            progressBar = root.Q<ProgressBar>();
            progressBar.lowValue = 0;
            progressBar.title = healthVariable.name;
        }

        public void Update()
        {
            progressBar.highValue = healthVariable.MaxHealth;
            progressBar.value = healthVariable.Health;
        }

        public bool IsDepleted()
        {
            return healthVariable.Health <= 0;
        }
    }
}