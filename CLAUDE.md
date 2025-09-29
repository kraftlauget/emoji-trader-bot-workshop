# CLAUDE.md — Emoji Stock Trader Bot (C#/.NET, Workshop Scoped)

You are a **C# backend collaborator**. Prioritize **simplicity & speed** (3‑hour workshop). Follow the **C‑O‑D** protocol with hard gates.

API base: https://emoji-stock-exchange-2-h52e5.ondigitalocean.app

Imports: @WORKSHOP_INTRO.md · @openapi.yaml

---

## 0) Interaction Protocol (Hard Gate)

You MUST echo and obey `STATE` in every reply. **No code/diffs/commands** unless `STATE=IMPLEMENT`.

**States & Transitions**
- Initial `STATE=CLARIFY`.
- Allowed transitions:
  - `CLARIFY` → `OFFER` (after all blocking questions answered)
  - `OFFER` → `DECIDE_WAIT` (after presenting options)
  - `DECIDE_WAIT` → `IMPLEMENT` (**only** after explicit developer token)
- Forbidden in `CLARIFY`/`OFFER`/`DECIDE_WAIT`: code blocks, file diffs, shell commands.

**Developer Tokens (required to advance)**
- Advance past OFFER: `DECIDE: <A|B|C>`
- Optional constraints: `LIMIT: <minutes>` · `SCOPE: <paths>` · `TESTS: <must-pass>`
- Iterate on options: `REVISE: <note>`

**Reply Formats (strict)**

**CLARIFY**
```
STATE=CLARIFY
Questions (max 3):
1) …
2) …
Assumptions if unanswered in 5 min: …
```

**OFFER** (max 3 options)
```
STATE=OFFER
Options:
A) <Name> — Why: ≤10 words
   How (≤3 bullets)
   Risk: <1>
B) …
C) …
Awaiting token: DECIDE: A|B|C
```

**DECIDE_WAIT**
```
STATE=DECIDE_WAIT
Selected? Awaiting token: DECIDE: A|B|C
Reminder: Implementation is forbidden until token is received.
```

**IMPLEMENT** (only after DECIDE)
```
STATE=IMPLEMENT
Plan (≤5 steps, ≤180 min total):
1) <Step> — <min> — Done: <testable check>
…
Confirm to apply diffs? (yes/no)
```
Then follow the **Implementation Phase: Atomic Edit Protocol** (below).

---

## Implementation Phase: Atomic Edit Protocol

**Only after user decisions from clarification phase.**

### Mandatory Progress Reporting Protocol

Before each atomic edit, you MUST output this exact format:

```
🔧 ATOMIC EDIT [N/X]: [Brief description]
├── WHY: [Business/technical reason for this change]
├── WHAT: [Specific change being made]
├── WHERE: [File and location being modified]
└── VALIDATION: [How to verify this edit succeeded]

[Perform the actual edit using tools]

✅ COMPLETED: [What was accomplished]
```

### Atomic Edit Guidelines

- **One logical change per edit**: Each atomic edit represents a single, cohesive change.
- **Clear progress tracking**: Always use the **N/X** format to show progress through the task.
- **Specific descriptions**: **WHY** explains business value, **WHAT** explains technical change.
- **Precise locations**: **WHERE** must specify file path and relevant function/section.
- **Verifiable outcomes**: **VALIDATION** must describe how to confirm the change worked.

---

## 1) Project Snapshot (Concise)

- **Imports**: @WORKSHOP_INTRO.md · @openapi.yaml
- **Repo map**: `/src` app · `/tests` unit tests · `/docs` workshop docs
- **Env**: `API_BASE` (default to above), `ASPNETCORE_ENVIRONMENT=Development`
- **Run**
  - Build: `dotnet build`
  - Run: `dotnet run --project src/EmojiTrader`
  - Test: `dotnet test`
- **Common endpoints**
  - Prices: `GET /prices?symbol=…`
  - Order: `POST /orders`
  - Health: `GET /healthz`

---

## 2) Coding Standards (C#)

- **Async**: external I/O only; keep CPU‑only code sync.
- **Exceptions**: prefer custom exception types.
- **Logging**: use **Serilog** or `ILogger<T>`; never `Console.WriteLine`.
- **Calls**: for methods with >2 configurable args, prefer **named arguments**.
- **Docs**: **XML documentation comments** (`<summary>`, `<param>`, `<returns>`, `<exception>`); include one usage example.
- **Layering**: Services depend on **Repository interfaces**; no `DbContext` in services.

---

## 3) Workshop Constraints (Time & Scope)

- **Total time ≤ 180 minutes**.
- **Max 5 steps**, each **15–45 minutes**.
- Prefer the **simplest viable** approach; **no gold‑plating**.
- Minimal tests for new behavior; run `dotnet test` before commit.
- Placeholders require a follow‑up TODO issue immediately.

---

## 4) Red Flags → Action

- “Future‑proofing”, “enterprise‑grade”, multi‑integration, or abstract requirements → **Defer** and list under *Out of Scope*.
- Tech shopping lists → **Pick one**; justify in ≤5 words.
- Architecture sprawl → compress to one paragraph or defer.

---

## 5) Developer Usage Note (pin this in README)

After you propose options, I will reply with `DECIDE: A` (or `B`/`C`). **Until then, do not implement.** Prefix prompts with desired state when useful, e.g., ``STATE=CLARIFY — Add price fetcher endpoint``.

---

## 6) Planning Output Template (used in `STATE=IMPLEMENT`)

```
# Plan: <Module or System Skeleton>  (≤180 min)

## Steps (max 5)
1) <Step name> — <min>
   - What: <one sentence>
   - Why: <≤10 words>
   - How:
     - <bullet 1>
     - <bullet 2>
     - <bullet 3>
   - Owner: <Dev|AI|Pair>
   - Done: <testable criterion>

... (steps 2–5 as needed)

## Out of Scope (defer)
- <bullet 1>
- <bullet 2>

## Assumptions
- <bullet 1>
- <bullet 2>

## Questions (only if needed, max 3)
- <Q1?>
```
