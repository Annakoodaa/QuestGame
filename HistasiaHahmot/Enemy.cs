using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestGame
{
    class Enemy
    {
        // Fields.
        protected string _name;
        protected int _health;
        protected int _defense;
        protected int _attackBonus;

        // Properties.
        public string Name
        {
            get { return _name; }
        }
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        public int Defense
        {
            get { return (_defense); }
        }
        public int AttackBonus
        {
            get { return _attackBonus; }
        }

        // Constructors.
        public Enemy(string name, int _health, int defense, int attackbonus)
        {
            _name = name;
            _health = _health;
            _defense = defense;
            _attackBonus = attackbonus;
        }
        public Enemy(Enemy enemy)
        {
            _name = enemy.Name;
            _health = enemy.Health;
            _defense = enemy.Defense;
            _attackBonus = enemy.AttackBonus;
        }
    }
}