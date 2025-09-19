# GitHub Copilot Instructions for RimWorld Modding Project: Smarter Visitors

Welcome to the development of the Smarter Visitors mod for RimWorld! This guide outlines the mod's functionality and provides coding guidelines to assist developers using GitHub Copilot in contributing to this project.

## Mod Overview and Purpose

The Smarter Visitors mod aims to enhance the visitor experience in RimWorld by giving groups of visitors smarter decision-making capabilities about when to leave your colony. Visitors will now consider various in-game conditions before deciding to depart, making their stay more logical and minimizing unnecessary risks.

## Key Features and Systems

- **Condition-Based Departure Delays:** Visitors will delay their departure based on map conditions like active raids, toxic fallout, or other dangers. They can request to stay longer until it's safe to leave.
  
- **Health-Related Decisions:** Visitors with health issues requiring medical rest will delay leaving until they are healthy enough to travel.

- **UV-Sensitivity Consideration:** Characters with UV-sensitivity, including certain vampire characters, will postpone their departure until nightfall.

- **Visitor Spot Designation:** Players can designate specific spots where visitors will gather, keeping them safe from potential threats.

- **Negative Thought Management:** If visitors stay longer than planned, they might gain negative thoughts, which worsen with prolonged delays. Food rations can help mitigate these effects.

- **Compatibility with Hospitality Mod:** This mod works seamlessly with the Hospitality mod by borrowing and adapting its danger-check logic.

## Coding Patterns and Conventions

- **Namespaces:** Ensure all classes are appropriately namespaced. Organize code into specific folders and namespaces to keep the project maintainable.
  
- **Class Structure:** Most classes should follow the model of having a central logic class accompanied by settings and configuration classes.

- **Comments and Documentation:** Use XML comments to document methods and classes thoroughly. This practice helps in maintenance and when using tools like GitHub Copilot to understand the purpose and usage of various components.

## XML Integration

- Integration with XML involves using Defs. Make sure to define any custom Defs, such as `ThoughtDef` or `BuildingDef`, in appropriate XML files under the Defs folder.
  
- Pay attention to ensuring that the XML data corresponds to the C# representation, maintaining consistent string identifiers between the two.

## Harmony Patching

- **Purpose of Harmony:** This project uses Harmony to modify existing RimWorld behaviors safely and effectively. Apply patches to game methods to extend or change their functionality as needed.

- **Implementation:** Use `[HarmonyPatch]` attributes to apply patches. Ensure patches are non-destructive and include fallback mechanisms when conditions aren't met.

- **Debugging Patches:** Copilot users should write thorough logging around patched code. This is crucial for diagnosing issues arising from compatibility or logic errors.

## Suggestions for Copilot

To enhance productivity with GitHub Copilot while contributing to this mod:

1. **Ask Copilot to generate boilerplate code:** Take advantage of Copilot's ability to generate class structures, particularly those that align with common design patterns used in RimWorld modding.
   
2. **Use Copilot for XML-template creation:** Request Copilot to generate XML templates based on existing Defs. This can speed up the process of introducing new game objects.

3. **Prompt Copilot for alternatives:** If a generated code snippet doesn't fit, ask Copilot for an alternative to explore different implementation strategies.

4. **Focus suggestions on logic flow:** Ask Copilot to complete sections of the AI decision-making logic to ensure smooth flow and integration.

5. **Leverage Copilot for documentation:** Use Copilot to generate initial XML comments, which you can refine and expand upon.

By adhering to these guidelines, you can ensure the continuous improvement and expansion of the Smarter Visitors mod, providing players a more refined and enjoyable experience.
