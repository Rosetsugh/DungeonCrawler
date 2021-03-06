﻿//#define CHILDDEBUG
//#define COMPONENTDEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GameFramework {
    class GameObject {
        public string Name = null;
        public Point GlobalPosition {
            get {
                if (Parent == null) {
                    return LocalPosition;
                }
                return new Point(LocalPosition.X + Parent.GlobalPosition.X, LocalPosition.Y + Parent.GlobalPosition.Y);
            }
        }
        public Point LocalPosition = new Point(0, 0);
        private bool _enabled = false;
        public bool Enabled {
            get {
                return _enabled;
            }
            set {
                if (value != _enabled) {
                    if (value) {
                        OnEnable();
                    }
                    else {
                        OnDisable();
                    }
                }
                _enabled = value;
            }
        }
        public List<GameObject> Children = null;
        public GameObject Parent {
            get; private set;
        }
        public List<Component> Components = null;
        public GameObject(string name) {
            Name = name;
            Enabled = true;
        }

        public void Update(float dTime) {
            if (Enabled) {
                //do self update stuff here
                if (Components != null) {
                    for (int i = Components.Count - 1; i >= 0; i--) {
                        if (Components[i].Active) {
                            Components[i].DoUpdate(dTime);
                        }
                    }
                }
                if (Children != null) {
                    for (int i = Children.Count - 1; i >= 0; i--) {
                        Children[i].Update(dTime);
                    }
                }
                
            }
            
        }
        public void Render() {
            if (Enabled) {
                //do self render stuff here
                if (Components != null) {
                    for (int i = 0; i < Components.Count;i++) {
                        if (Components[i].Active) {
                            Components[i].DoRender();
                        }
                    }
                }
                if (Children != null) {
                    for (int i = 0; i < Children.Count; i++) {
                        Children[i].Render();
                    }
                }

                //GraphicsManager.Instance.DrawString(Name, GlobalPosition,  Name== "Skill3IdentifierObj"?Color.Red : Color.Black);

            }
        }

        public void OnEnable() {

        }
        public void OnDisable() {

        }
        public void AddChild(GameObject child) {
            if (Children == null) {
                Children = new List<GameObject>();
            }
            if (child.Parent != null) {
                child.Parent.RemoveChild(child);
            }
            Children.Add(child);
            child.Parent = this;
#if CHILDDEBUG
            Console.WriteLine("Added child:" + child.ToString());
            Console.WriteLine("Children Length: " + Children.Count);
#endif
        }

        public void RemoveChild(GameObject child) {
            for (int i = Children.Count - 1; i >= 0; i--) {
                if (Children[i] == child) {
                    child.Parent = null;
                    Children.RemoveAt(i);
#if CHILDDEBUG
                    Console.WriteLine("Removed child:" + child.ToString() + " at: " + i);
                    Console.WriteLine("Children Length: " + Children.Count);
#endif
                }
            }
        }

        public void RemoveChild(string child, bool recursive = false) {
            for (int i = Children.Count - 1; i >= 0; i--) {
                if (Children[i].Name == child) {
                    Children.RemoveAt(i);
#if CHILDDEBUG
                    Console.WriteLine("Removed child:" + child.ToString() + " at: " + i);
                    Console.WriteLine("Children Length: " + Children.Count);
#endif
                }
                else if (recursive) {
                    Children[i].RemoveChild(child, true);
                }
            }
        }

        public GameObject FindChild(string child, bool recursive = true) {
            if (Children == null) {
#if CHILDDEBUG
                Console.WriteLine("Child not found");
#endif
                return null;
            }
            for (int i = Children.Count-1; i >= 0; i--) {
                if (Children[i].Name == child) {
#if CHILDDEBUG
                    Console.WriteLine("Child found at: " + i);
#endif
                    return Children[i];
                }
                else if (recursive) {
                    GameObject inChild = Children[i].FindChild(child);
                    if (inChild != null) {
                        return inChild;
                    }
                }
            }
#if CHILDDEBUG
            Console.WriteLine("Child not found");
#endif
            return null;
        }
        public void AddComponent(Component component) {
            if (Components == null) {
                Components = new List<Component>();
            }
            for (int i = 0; i < Components.Count ; i++) {
                if (Components[i].Name == component.Name) {
#if COMPONENTDEBUG
                    Console.WriteLine("Component already exists, was not added: "+component.Name);
#endif
                    return;
                }
            }
            Components.Add(component);
#if COMPONENTDEBUG
            Console.WriteLine("Component added: " + component);
            Console.WriteLine("Component Length: " + Components.Count);
#endif
        }
        public void RemoveComponent(Component component) {
            for (int i = Components.Count - 1; i >= 0; i--) {
                if (Components[i] == component) {
                    Components.RemoveAt(i);
#if COMPONENTDEBUG
                    Console.WriteLine("Component removed: " + component);
                    Console.WriteLine("Component Length: " + Components.Count);
#endif
                }
            }
        }
        public void RemoveComponent(string component) {
            for (int i = Components.Count - 1; i >= 0; i--) {
                if (Components[i].Name == component) {
                    Components.RemoveAt(i);
#if COMPONENTDEBUG
                    Console.WriteLine("Component removed: " + component);
                    Console.WriteLine("Component Length: " + Components.Count);
#endif
                }
            }
        }
        public Component FindComponent(String component) {
            for (int i = Components.Count - 1; i >= 0; i--) {
                if (Components[i].Name == component) {
#if COMPONENTDEBUG
                    Console.WriteLine("Component Found: " + component + " at: " + i);
#endif
                    return Components[i];
                }
            }
#if COMPONENTDEBUG
            Console.WriteLine("Component not found");
#endif
            return null;
        }

        public void Destroy() {
            if (Components != null) {
                for (int i = Components.Count - 1; i >= 0; i--) {
                    Components[i].DoDestroy();
                }
            }
            if (Children != null) {
                for (int i = Children.Count - 1; i >= 0; i--) {
                    Children[i].Destroy();
                }
            }
        }
    }
}
