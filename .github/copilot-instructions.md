# GitHub Copilot Instructions for Smarter Visitors Mod

## Mod Overview and Purpose

The **Smarter Visitors** mod is designed to enhance the interaction with visitor groups in RimWorld by providing smarter decision-making processes for when visitors decide to leave your colony. Visitors will now make informed choices about their departure, taking into account various environmental and health conditions. This mod adds a level of realism and strategy, ensuring that the departure of visitors is timed more appropriately based on situational factors.

## Key Features and Systems

- **Visitor Departure Decision:** Visitors will assess the map for dangers such as raids, maddened animals, or toxic fallout before deciding to leave. They will also consider their own health and UV sensitivity.
  
- **Visitor Spot:** Designate a specific spot for visitors to congregate, making it easier to protect them from potential threats.

- **Health and UV Sensitivity Checks:** Visitors will delay their departure if any member of their group is unfit for travel due to health issues or UV sensitivity.

- **Mood Impact on Delays:** Visitors may develop negative thoughts if they stay longer than anticipated, which intensifies with extended delays.

- **Harmony and Compatibility:** The mod works seamlessly with the Hospitality mod by Orion, adapting similar logic for assessing map dangers.

## Coding Patterns and Conventions

- **Convention Adherence:** The source code follows C# conventions, such as PascalCase for class names and camelCase for method and variable names.

- **Code Structure:** Classes are designed to be modular, with clear separation of concerns. Each file generally defines one class with specific functionality.

- **Comments and Documentation:** Key sections of the code are commented to enhance clarity, keeping in mind future extensibility.

## XML Integration

- **Definitions:** XML files define custom ThingDefs, JobDefs, and ThoughtDefs to integrate new behaviors and items related to visitor interactions.

- **Localization Support:** XML files are structured to support translations, ensuring mod readiness for international audiences.

## Harmony Patching

- **Dependency:** The mod relies on the Harmony library (brrainz.harmony) to perform method patches that alter the behavior of the base game code.

- **Method Patching:** Use Harmony's prefix and postfix patching to customize the logic for visitor departure without directly modifying the game’s original methods.

- **Method Safety:** Ensure patch methods are idempotent and non-destructive to maintain compatibility with other mods.

## Suggestions for Copilot

- **Class and Method Drafting:** Copilot can assist in creating new classes or methods by suggesting standard templates and patterns.

- **XML Template Generation:** Utilize Copilot to draft new XML def templates for expanding mod features.

- **Harmony Patch Templates:** Generate patch classes and methods with placeholders for customization.

- **Optimization Suggestions:** Leverage Copilot to provide optimizations in runtime operations or suggest refactoring opportunities in complex functions.

By utilizing these guidelines and Copilot’s assisted code generation, developers can efficiently expand and maintain the Smarter Visitors mod, ensuring a seamless and feature-rich experience for RimWorld players.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

