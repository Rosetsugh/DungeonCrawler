﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework {
    class Component {
        protected GameObject gameObject = null;
        public string Name = null;
        private bool _active = true;
        public bool Active {
            get {
                return _active;
            }
            set {
                if (value != _active) {
                    if (value) {
                        OnActivate();
                    }
                    else {
                        OnDeactivate();
                    }
                }
                _active = value;
            }
        }
        public Component(string name, GameObject game) {
            Name = name;
            gameObject = game;
            gameObject.AddComponent(this);
            OnActivate();
        }
        public virtual void OnActivate() {

        }

        public virtual void OnDeactivate() {

        }

        public virtual void OnUpdate(float dTime) {

        }

        public virtual void OnRender() {

        }
        public virtual void OnDestroy() {

        }
        public void DoDestroy() {
            OnDestroy();
        }

        public void DoUpdate(float dTime) {
            if (Active) {
                OnUpdate(dTime);
            }
        }

        public void DoRender() {
            if (Active) {
                OnRender();
            }
        }
    }
}
