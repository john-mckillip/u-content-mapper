# Commit Message Guide

This guide defines how an AI agent must write commit messages for work it completes.
It follows the conventions popularized in Chris Beams’ summary of Tim Pope’s guidance (tbaggery): clear subject line, imperative mood, blank separator line, and explanatory body.

## Core Rules (Required)

1. **Use a short subject line (≤ 50 chars preferred).**
2. **Capitalize the subject line.**
3. **Do not end the subject line with a period.**
4. **Write the subject in imperative mood** ("Add", "Fix", "Refactor", not "Added" or "Fixes").
5. **Leave one blank line between subject and body.**
6. **Wrap body lines at ~72 characters.**
7. **Explain what and why, not only how.**

## Agent-Specific Expectations

When an agent has completed work, the commit message body should include:

- **Scope**: what components/files changed.
- **Behavioral outcome**: what is now true after the change.
- **Reasoning**: why this change was needed (bug, review feedback, coverage, etc.).
- **Validation**: tests run and outcomes.

Avoid low-signal bodies like:

- "update files"
- "fix stuff"
- "address comments"

## Subject Line Pattern

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

## Body Pattern

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

## Multi-Concern Work

If work spans unrelated concerns, split into multiple commits.
One commit should represent one logical change set.

## Pre-Commit Checklist (Agent)

Before finalizing a message, verify:

- Subject is imperative and concise.
- Subject has no trailing period.
- Blank line exists between subject and body.
- Body explains **why** the change exists.
- Body includes test/validation summary when applicable.
- Lines are wrapped for readability (~72 chars).

## Default Template

Use this when generating commit text:

```
<Imperative subject, <= 50 chars>

<What changed.>
<Why this was necessary.>

<Validation performed and result.>
```
