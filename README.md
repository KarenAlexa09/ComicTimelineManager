# ComicTimelineManager

## Overview
The **ComicTimelineManager** is responsible for managing and displaying interactive comic-based story snippets in Unity. It allows players to unlock and view different story fragments as they progress through the game.

## Classes and Responsibilities

### 1. **UIStoryView**
This class handles the UI for displaying available story snippets.

#### **Main Responsibilities:**
- Initializes and manages the story snippet gallery.
- Plays animations for UI elements.
- Detects user interaction with story snippets.
- Invokes the `PlayedTimeline` event when a snippet is opened.

---

### 2. **StoryCardUIView**
Represents an individual story snippet card in the UI.

#### **Main Responsibilities:**
- Displays snippet details such as title, image, and progress.
- Manages locked/unlocked states.
- Handles click events to open snippets.
- Plays UI animations for smooth visual effects.

---

### 3. **StorySnippetsModel (ScriptableObject)**
Stores a collection of `StorySnippet` objects.

#### **Main Responsibilities:**
- Acts as a database for story snippets.
- Provides access to stored snippets.

---

### 4. **StorySnippet**
Represents an individual story snippet with attributes and metadata.

#### **Main Responsibilities:**
- Holds snippet data such as title, progress, unlock state, and associated visuals.
- Provides methods for modifying unlock and notification states.

---

## Dependencies
- **DoTween**: Used for smooth UI animations.
- **TextMeshPro**: Handles UI text rendering.
- **Unity Addressables**: Manages asset references for playable content.
- **Timeline**.

## Usage
1. **Setup in Unity:**
   - Drag on the scene the prefab `Canvas_Comic_Controller` to the scene.
   - Assign `StorySnippetsModel` and configurate.
2. **Loading Snippets:**
   - Call `Initialize(List<StorySnippet>)` with a list of unlocked snippets.

## Notes
- See an example in the `SampleEscene`.
- Ensure `StorySnippetsModel` contains all available story snippets before calling `Initialize()`.
- Unlocked snippets should be determined by the game’s progress tracking system.
- `PlayableDirectorPrefab` should be properly referenced to play story animations.

## Contributions & Contact
If you want to improve this system or report issues, feel free to contribute or get in touch.

This document may be updated with more details as the system evolves.

