using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public class SimpleAccordion
    {
        public List<Chapter> Chapters = new List<Chapter>();
        public bool SingleChapter;

        public void OnGui()
        {
            GUILayout.BeginVertical();
            for (int index1 = 0; index1 < Chapters.Count; ++index1)
            {
                Chapter chapter = Chapters[index1];
                GUIStyle style = chapter.Opened ? TNBaseStyle.FoldoutOpen : TNBaseStyle.FoldoutClose;
                if (GUILayout.Button(chapter.Title, style))
                {
                    chapter.Opened = !chapter.Opened;
                    if (chapter.Opened && SingleChapter)
                    {
                        for (int index2 = 0; index2 < Chapters.Count; ++index2)
                        {
                            if (index1 != index2)
                                Chapters[index2].Opened = false;
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

        public void AddChapter(string title, OnChapterUI chapterUI) => Chapters.Add(new Chapter(title, chapterUI));

        public int Count => Chapters.Count;

        public delegate void OnChapterUI();

        public class Chapter
        {
            public string Title;
            public OnChapterUI ChapterUI;
            public bool Opened;

            public Chapter(string Title, OnChapterUI chapterUI)
            {
                this.Title = Title;
                ChapterUI = chapterUI;
            }
        }
    }
}