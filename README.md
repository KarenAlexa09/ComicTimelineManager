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

#### **Key Methods:**
- `Initialize(List<StorySnippet> _snippets)`: Sets up the UI with available snippets.
- `PlayAnimations()`: Plays the introduction animation.
- `OnSnippetOpen(StorySnippet snippet)`: Handles snippet selection.
- `Conclude()`: Cleans up event listeners and UI elements.

---

### 2. **StoryCardUIView**
Represents an individual story snippet card in the UI.

#### **Main Responsibilities:**
- Displays snippet details such as title, image, and progress.
- Manages locked/unlocked states.
- Handles click events to open snippets.
- Plays UI animations for smooth visual effects.

#### **Key Methods:**
- `Initialize(StorySnippet _snippet)`: Loads the snippet's data.
- `UpdateSnippetState(bool isUnlocked)`: Updates the card's lock state.
- `OnSnippetOpen(StorySnippet snippet)`: Triggers when the snippet is clicked.
- `PlayAnimations()`: Animates the snippet card when displayed.
- `Conclude()`: Cleans up event listeners.

---

### 3. **StorySnippetsModel (ScriptableObject)**
Stores a collection of `StorySnippet` objects.

#### **Main Responsibilities:**
- Acts as a database for story snippets.
- Provides access to stored snippets.

#### **Key Methods:**
- `GetStory()`: Returns the list of story snippets.

---

### 4. **StorySnippet**
Represents an individual story snippet with attributes and metadata.

#### **Main Responsibilities:**
- Holds snippet data such as title, progress, unlock state, and associated visuals.
- Provides methods for modifying unlock and notification states.

#### **Key Properties:**
- `StoryType`: Type of the story snippet.
- `IsUnlocked`: Indicates if the snippet is unlocked.
- `CoverImage`: The snippet's visual representation.
- `Description`: A short description of the snippet.
- `PlayableDirectorPrefab`: Reference to the playable animation.

#### **Key Methods:**
- `SetStorySnippetUnlocked(bool _isUnlocked, StoryEntity storyEntity)`: Unlocks a snippet.
- `SetStorySnippetNotified(bool _isNotified, StoryEntity storyEntity)`: Marks a snippet as notified.

## Dependencies
- **DoTween**: Used for smooth UI animations.
- **TextMeshPro**: Handles UI text rendering.
- **Unity Addressables**: Manages asset references for playable content.

## Usage
1. **Setup in Unity:**
   - Attach `UIStoryView` to a UI container in the scene.
   - Assign `StorySnippetsModel` in the Unity Inspector.
2. **Loading Snippets:**
   - Call `Initialize(List<StorySnippet>)` with a list of unlocked snippets.
3. **Playing Animations:**
   - Call `PlayAnimations()` to animate snippet cards.
4. **Handling Snippet Selection:**
   - The `PlayedTimeline` event triggers when a snippet is opened.

## Notes
- Ensure `StorySnippetsModel` contains all available story snippets before calling `Initialize()`.
- Unlocked snippets should be determined by the game’s progress tracking system.
- `PlayableDirectorPrefab` should be properly referenced to play story animations.

## Future Improvements
- Add support for saving snippet unlock states across sessions.
- Implement transitions between snippet playback and game UI.
- Optimize animation timing for better user experience.

