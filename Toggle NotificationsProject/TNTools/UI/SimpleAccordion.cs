
using System.Collections.Generic;
using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public class SimpleAccordion
    {
        public List<SimpleAccordion.Chapter> Chapters = new List<SimpleAccordion.Chapter>();
        public bool SingleChapter;

        public void OnGui()
        {
            GUILayout.BeginVertical();
            for (int index1 = 0; index1 < this.Chapters.Count; ++index1)
            {
                SimpleAccordion.Chapter chapter = this.Chapters[index1];
                GUIStyle style = chapter.Opened ? TNBaseStyle.FoldoutOpen : TNBaseStyle.FoldoutClose;
                if (GUILayout.Button(chapter.Title, style))
                {
                    chapter.Opened = !chapter.Opened;
                    if (chapter.Opened && this.SingleChapter)
                    {
                        for (int index2 = 0; index2 < this.Chapters.Count; ++index2)
                        {
                            if (index1 != index2)
                                this.Chapters[index2].Opened = false;
                        }
                    }
                }
                if (chapter.Opened)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20f);
                    GUILayout.BeginVertical();
                    chapter.ChapterUI();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }

        public void AddChapter(string title, SimpleAccordion.OnChapterUI chapterUI) => this.Chapters.Add(new SimpleAccordion.Chapter(title, chapterUI));

        public int Count => this.Chapters.Count;

        public delegate void OnChapterUI();

        public class Chapter
        {
            public string Title;
            public SimpleAccordion.OnChapterUI ChapterUI;
            public bool Opened;

            public Chapter(string Title, SimpleAccordion.OnChapterUI chapterUI)
            {
                this.Title = Title;
                this.ChapterUI = chapterUI;
            }
        }
    }
}