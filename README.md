# Trox Delivery

Un camionneur de l’extrême doit livrer des cargaisons importantes dans des conditions difficiles. 

## Démarrage rapide

Ces instructions vous permettront d'obtenir une copie opérationnelle du projet sur votre machine à des fins de développement.

### Prérequis

* [Git](https://git-scm.com/downloads) - Système de contrôle de version. Utilisez la dernière version.
* [Rider](https://www.jetbrains.com/rider/) ou [Visual Studio](https://www.visualstudio.com/fr/) - IDE. Vous pouvez utiliser 
  également n'importe quel autre IDE: assurez-vous simplement qu'il supporte les projets Unity.
* [Unity 2020.1.7f1](https://unity3d.com/fr/get-unity/download/) - Moteur de jeu. Veuillez utiliser **spécifiquement cette 
  version.** Attention à ne pas installer Visual Studio une seconde fois si vous avez déjà un IDE.

**Attention!** Actuellement, seul le développement sur Windows est complètement supporté.

### Compiler une version de développement

Clonez le projet.

```
git clone https://gitlab.com/davidp027/trox-delivery.git
```

Ouvrez le projet dans Unity. Ensuite, ouvrez la scène `Main` et appuyez sur le bouton *Play*.

### Tester un version stable ou de développement

Ouvrez le projet dans Unity. Ensuite, allez dans `File > Build Settings…` et compilez le projet **dans un dossier vide**.

Si vous rencontrez un bogue, vous êtes priés de le [signaler](https://gitlab.com/davidp027/trox-delivery/issues/new?issuable_template=Bug).
Veuillez fournir une explication détaillée de votre problème avec les étapes pour reproduire le bogue. Les captures d'écran et 
les vidéos jointes sont les bienvenues.

## Contribuer au projet

Veuillez lire [CONTRIBUTING.md](CONTRIBUTING.md) pour plus de détails sur notre code de conduite.

## Auteurs

***TODO** : Ajoutez vous noms ici ainsi que le nom de tout artiste ayant participé au projet (avec lien vers leur portfolio s'il existe).
Inscrivez aussi, en détail, ce sur quoi chaque membre de l'équipe a principalement travaillé.*

* **Benjamin Lemelin** - *Programmeur*
  * Extensions sur le moteur Unity pour la recherche d'objets et de composants. Générateur de constantes. Gestionnaire de
    chargement des scènes.
* **Benoit Simon-Turgeon** - *Programmeur*
* **Félix Bernier** - *Programmeur*
* **François-Xavier Bernier** - *Programmeur*
* **Seyed-Rahmatoll Javadi** - *Programmeur*
* **David Pagotto** - *Programmeur*

## Remerciements

* Tyler Coles - Pour [son guide](https://ornithoptergames.com/how-to-set-up-sqlite-for-unity/) d'intégration de SQLite dans Unity, dont l'implémentation dans ce projet fut fortement inspirée.
