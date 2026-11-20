# Mobile_App

A campus map app that helps people with visual impairments find their way around the Hanze campus. This is a 10-week group project.

## Setup
- **Unity version: `6000.3.16f1`.** Everyone must use exactly this version. Install it through Unity Hub.
- Install [Git LFS](https://git-lfs.com) once with `git lfs install`. Models, images, audio and fonts are stored with LFS.
- Clone the repo, then open the folder in Unity Hub with **Add → Add project from disk**.

## Branch workflow
1. Pull the latest `main`: `git checkout main` then `git pull`.
2. Make your own branch: `git checkout -b yourname/feature-name`.
3. Commit and push to **your branch only**: `git push -u origin yourname/feature-name`.
4. Open a Pull Request into `main` before Sunday.
5. **Every Sunday** the project lead merges the branches into `main` for the sprint review. After that, everyone pulls `main` again.

Never push directly to `main`.

### Avoiding merge conflicts
- Agree on who edits which **scene**. Two people editing the same `.unity` scene file at once is the most common cause of Unity conflicts. Use prefabs, or a separate test scene per person.
- Always commit the `.meta` files together with their assets.
