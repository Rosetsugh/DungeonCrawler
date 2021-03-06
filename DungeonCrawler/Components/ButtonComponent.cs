﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFramework;

namespace Components {
    class ButtonComponent : Component{
        public delegate void ClickCallback();
        public delegate void HoverCallback();
        public delegate void NonHoverCallback();
        public ClickCallback DoClick = null;
        public HoverCallback DoHover = null;
        public NonHoverCallback NoHover = null;
        protected bool wasPushed = false;
        public int xOffset = 0;
        public int yOffset = 0;
        public int wOffset = 0;
        public int HOffset = 0;
        //gameobject location
        //image size of button, get component of the button
        public StaticSpriteRendererComponent StaticRenderer;
        public AnimatedSpriteRendererComponent AnimRenderer;
        public ButtonComponent(GameObject game):base("ButtonComponent", game) {
        }

        public override void OnActivate() {
            StaticRenderer = (StaticSpriteRendererComponent)gameObject.FindComponent("StaticSpriteRendererComponent");
            AnimRenderer = (AnimatedSpriteRendererComponent)gameObject.FindComponent("AnimatedSpriteRendererComponent");
        }
        public override void OnUpdate(float dTime) {
            //if mouse is in position?
            if (Active) {
                InputManager i = InputManager.Instance;
                int width = 0;
                int height = 0;
                int xPos = gameObject.GlobalPosition.X;
                int yPos = gameObject.GlobalPosition.Y;
                if (StaticRenderer != null) {
                    width = StaticRenderer.SourceRects[StaticRenderer.CurrentSprite].Width;
                    height = StaticRenderer.SourceRects[StaticRenderer.CurrentSprite].Height;
                }
                else if (AnimRenderer != null) {
                    width = AnimRenderer.AnimationBank[AnimRenderer.CurrentAnimation][AnimRenderer.CurrentFrame].Width;
                    height = AnimRenderer.AnimationBank[AnimRenderer.CurrentAnimation][AnimRenderer.CurrentFrame].Width;
                }
                //is x inside?
                width += wOffset;
                height += HOffset;
                xPos += xOffset;
                yPos += yOffset;
                if (xPos < i.MousePosition.X && i.MousePosition.X < (xPos+width) && yPos < i.MousePosition.Y && i.MousePosition.Y < (yPos + height)) {
                    if (DoHover != null) {
                            DoHover();
                    }
                    if (DoClick != null) {
                        if (i.MousePressed(OpenTK.Input.MouseButton.Left)) {
                            wasPushed = true;
                        }
                        if (i.MouseReleased(OpenTK.Input.MouseButton.Left ) && wasPushed) {
                            DoClick();
                            wasPushed = false;
                        }
                    }
                }//end x
                else {
                    if (NoHover != null) {
                        NoHover();
                    }
                }//end else
                if (i.MouseUp(OpenTK.Input.MouseButton.Left)) {
                    wasPushed = false;
                }
            }

        }//end update
        /*public override void OnRender() {
            base.OnRender();
            int width = 0;
            int height = 0;
            int xPos = gameObject.GlobalPosition.X;
            int yPos = gameObject.GlobalPosition.Y;
            if (StaticRenderer != null) {
                width = StaticRenderer.SourceRects[StaticRenderer.CurrentSprite].Width;
                height = StaticRenderer.SourceRects[StaticRenderer.CurrentSprite].Height;
            }
            else if (AnimRenderer != null) {
                width = AnimRenderer.AnimationBank[AnimRenderer.CurrentAnimation][AnimRenderer.CurrentFrame].Width;
                height = AnimRenderer.AnimationBank[AnimRenderer.CurrentAnimation][AnimRenderer.CurrentFrame].Width;
            }
            //is x inside?
            width += wOffset;
            height += HOffset;
            xPos += xOffset;
            yPos += yOffset;
            Rectangle outline = new Rectangle(xPos, yPos, width, height);
            GraphicsManager.Instance.DrawRect(outline, Color.Red);
        }*/
    }
}
