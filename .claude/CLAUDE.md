## Workflow Orchestration

### 1. Plan Mode Default

- Enter plan mode for ANY non-trivial task (3+ steps or architectural decisions)
- If something goes sideways, STOP and re-plan immediately - don't keep pushing
- Use plan mode for verification steps, not just building
- Write detailed specs upfront to reduce ambiguity

### 2. Subagent Strategy

- Use subagents liberally to keep main context window clean
- Offload research, exploration, and parallel analysis to subagents
- For complex problems, throw more compute at it via subagents
- One tack per subagent for focused execution

### 3. Self-Improvement Loop

- After ANY correction from the user: update `tasks/lessons.md` with the pattern
- Write rules for yourself that prevent the same mistake
- Ruthlessly iterate on these lessons until mistake rate drops
- Review lessons at session start for relevant project

### 4. Verification Before Done

- Never mark a task complete without proving it works
- Diff behavior between main and your changes when relevant
- Ask yourself: "Would a staff engineer approve this?"
- Run tests, check logs, demonstrate correctness

### 5. Demand Elegance (Balanced)

- For non-trivial changes: pause and ask "is there a more elegant way?"
- If a fix feels hacky: "Knowing everything I know now, implement the elegant solution"
- Skip this for simple, obvious fixes - don't over-engineer
- Challenge your own work before presenting it

### 6. Autonomous Bug Fixing

- When given a bug report: just fix it. Don't ask for hand-holding
- Point at logs, errors, failing tests - then resolve them
- Zero context switching required from the user
- Go fix failing CI tests without being told how

## Task Management

1. **Plan First**: Write plan to `tasks/todo.md` with checkable items
2. **Verify Plan**: Check in before starting implementation
3. **Track Progress**: Mark items complete as you go
4. **Explain Changes** High-level summary at each step
5. **Document Results** Add review section to `tasks/todo.md`
6. **Capture Lessons** Update `tasks/lessons.md` after corrections

## Core Principals

- **Simplicity First**: Make every change as simple as possible. Impact minimal code.
- **No Laziness**: Find root causes. No temporary fixes. Senior developer standards.
- **Minimal Impact**: Changes should only touch what's necessary. Avoid introducing bugs.
- **Edge Cases**: Err on the side of handling more edge cases, not fewer - thoughtfulness > speed
- **DRY is Important**: Flag and/or avoid repetion aggressively
- **Engineer Enough**: Write code that's "engineered enough" - not under-engineered (fragile, hacky) and not over-engineered (premature abstraction, unnecessary complexity)
- **Explicit Engineering**: Be bias toward explicit over clever

## Commit Messages

### Core Rules (Required)

1. **Use a short subject line (≤ 50 chars preferred).**
2. **Capitalize the subject line.**
3. **Do not end the subject line with a period.**
4. **Write the subject in imperative mood** ("Add", "Fix", "Refactor", not "Added" or "Fixes").
5. **Leave one blank line between subject and body.**
6. **Wrap body lines at ~72 characters.**
7. **Explain what and why, not only how.**

### Agent-Specific Expectations

When an agent has completed work, the commit message body should include:

- **Scope**: what components/files changed.
- **Behavioral outcome**: what is now true after the change.
- **Reasoning**: why this change was needed (bug, review feedback, coverage, etc.).
- **Validation**: tests run and outcomes.

Avoid low-signal bodies like:

- "update files"
- "fix stuff"
- "address comments"

### Subject Line Pattern

Use this format:

`<Imperative verb> <specific object>`

Good examples:

- `Fix MediaWithCrops exception handling`
- `Add integration test for missing property`
- `Refine resolver test to match runtime behavior`

Bad examples:

- `Fixed things in tests.`
- `updates`
- `WIP`

### Body Pattern

Use short paragraphs or bullets, wrapped at ~72 chars.

Recommended sections:

- **What changed**
- **Why it changed**
- **How it was validated**

Example:

```
Fix MediaWithCrops catch behavior

Replace the bare catch with a filtered InvalidOperationException
handler that only suppresses the expected missing IUrlProvider case.
Allow unexpected exceptions to propagate for easier diagnosis.

Add deterministic unit tests for null input, filtered fallback,
non-null resolver output, and unexpected exception propagation.

Validated with focused mapping tests (all passing).
```

### Multi-Concern Work

If work spans unrelated concerns, split into multiple commits.
One commit should represent one logical change set.

### Pre-Commit Checklist (Agent)

Before finalizing a message, verify:

- Subject is imperative and concise.
- Subject has no trailing period.
- Blank line exists between subject and body.
- Body explains **why** the change exists.
- Body includes test/validation summary when applicable.
- Lines are wrapped for readability (~72 chars).

### Default Template

Use this when generating commit text:

```
<Imperative subject, <= 50 chars>

<What changed.>
<Why this was necessary.>

<Validation performed and result.>
```
