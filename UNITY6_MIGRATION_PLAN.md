# Unity 6.3 Migration Plan (Initial Bring-up)

## Baseline Snapshot
- Original editor version: `2019.3.15f1` (`ProjectSettings/ProjectVersion.txt`)
- Current scenes in build:
  - `Assets/Scenes/Menu.unity`
  - `Assets/Scenes/Game.unity`
- Legacy mobile service plugins currently embedded:
  - Google Play Games plugin `0.10.09` (`Assets/GooglePlayGames/PluginVersion.cs`)
  - External Dependency Manager `1.2.153` (`Assets/ExternalDependencyManager/...`)
  - Legacy Android AAR/JAR set under `Assets/Plugins/Android`

## Changes Applied In This Pass
1. Removed legacy Unity Ads package dependency:
   - Removed `com.unity.ads` from `Packages/manifest.json`.
2. Removed compile-time Unity Ads API usage:
   - Reworked `Assets/Scripts/AdManager.cs` into a legacy no-op compatibility component.
   - No usage of `UnityEngine.Advertisements`, `IUnityAdsListener`, or `Advertisement.*` remains.
3. Removed gameplay ad trigger logic:
   - Removed interstitial ad counter/calls from `Assets/Scripts/Player.cs`.
4. Removed scene UI event that attempted to trigger rewarded ads:
   - Removed `PlayRewaredVideoAd` on-click call from `Assets/Scenes/Game.unity`.
5. Added small runtime hardening:
   - Null-check before leaderboard submission in `Assets/Scripts/Player.cs`.
   - Null-checks for visualizer audio source in:
     - `Assets/Scripts/Visualizer.cs`
     - `Assets/Scripts/VisualiserMenu.cs`

## Phase 1 Goal: Open + Play In Unity 6.3
1. Open the project in Unity 6.3 and let Unity complete:
   - Asset reimport
   - Script compilation
   - Package resolution
2. Fix any compile/import errors in this order:
   - Script compile errors in custom gameplay scripts
   - Package resolution errors from old package versions
   - Plugin/editor script errors from Google Play Games or EDM4U
3. Validate basic gameplay:
   - Menu scene loads
   - Game scene loads
   - Player movement, score, stairs, orb boost, game over, and respawn flow

## Likely Next Blockers (After Ads Removal)
1. Very old Google Play Games + EDM4U versions may not be stable on Unity 6.3.
2. Old Android dependency set in `Assets/Plugins/Android` may conflict with modern Gradle/Android tooling.
3. Package versions in `Packages/manifest.json` are pinned to 2019-era versions and may need upgrading to Unity 6-compatible versions.

## Recommended Strategy For Service Integrations
1. Keep gameplay bring-up separate from platform services.
2. If Google Play Games causes compile/import issues:
   - Temporarily disable leaderboard integration path.
   - Replace old embedded plugin with latest supported package-based integration after gameplay is stable.
3. Only after a stable editor/play mode:
   - Rebuild Android dependency graph (EDM4U / Gradle)
   - Update target SDK and signing/release pipeline.

## Phase 2 (After Game Runs)
1. Refactor singleton usage and scene wiring safety.
2. Replace string-based `Invoke` calls with `nameof(...)` where possible.
3. Remove dead fields and stale serialized properties.
4. Optimize object lifecycle:
   - Reduce per-frame allocations
   - Pool repeated effects if needed
5. Add basic smoke tests/checklist for future engine upgrades.

