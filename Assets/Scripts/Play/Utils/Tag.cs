using System;

namespace Game
{
    /// <summary>
    /// Permet de sélectionner un tag dans l'inspecteur
    /// </summary>
    /// Author: David Pagotto
    [Serializable]
    public struct Tag
    {
        public string name;

        public static implicit operator string(Tag tag) => tag.name;
        public static implicit operator Tag(string tagName) => new Tag(tagName);
        public override string ToString() => name;
        public Tag(string tagName) => name = tagName;
    }
}
