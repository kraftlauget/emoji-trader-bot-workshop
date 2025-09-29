# CLAUDE.md — Emoji Stock Trader Bot (C#/.NET)

You are a **C# backend** collaborator. Prioritize **simplicity & delivery speed** (3-hour workshop). Follow the **C-O-D loop** for every coding task.

API base: https://emoji-stock-exchange-2-h52e5.ondigitalocean.app

See @WORKSHOP_INTRO.md for workshop info and tasks. Read `openapi.yaml` for Open API spec.

## 0) Collaboration: C-O-D (CRITICAL)

- **CLARIFY** → If any requirement/assumption is <100% clear, ask 1-3 pointed questions.
- **OFFER** → Propose up to **3 options** (1-sentence pros/cons). **Do not implement yet.**
- **DECIDE** → **Wait** for choice; implement only after explicit confirmation.

### Option template

- **Option A — <name>**  
  Pros: … Cons: …
- **Option B — <name>**  
  Pros: … Cons: …
- **Option C — <name>**  
  Pros: … Cons: …

## 1) Project Snapshot

- **Imports**: @WORKSHOP_INTRO.md · @openapi.yaml
- **Repo map**: `/src` app code · `/docs` workshop docs
- **Run commands**
  - Build: `dotnet build`
  - Run: `dotnet run --project src/EmojiTrader`
  - Test: `dotnet test`
- **Common tasks**
  - Create order → POST `/v1/orders`
  - Get prices → GET `/v1/orderbook?symbol=...`

## 2) Coding Standards (C#)

- **Async**: external I/O only; keep pure CPU code sync.
- **Exceptions**: prefer custom exception types over generic.
- **Logging**: use **Serilog** (or `ILogger<T>`); **never** `Console.WriteLine`.
- **Parameters**: when methods have >2 configurable args, prefer **named arguments** at call sites.
- **Docs**: use **XML documentation comments** (`<summary>`, `<param>`, `<returns>`, `<exception>`); include 1 usage example.
- **Layering**: Services depend on **Repository interfaces**; no `DbContext` in services.

## 3) Workflow & Constraints

- **Timebox**: prefer the simplest viable approach; target ≤20 minutes per step.
- **Tests**: skip tests for this short workshop.
- **Dependencies**: **Do not add** a new package without asking.
- **Size limits**: no file > **600 LOC**, no function > **75 LOC**.

## 4) Common Pitfalls (avoid)

- Implementing before **DECIDE**.
- Over-engineering; follow **YAGNI**/**KISS**.
- Vague assumptions—**ask first**.
- Long-running operations without cancellation tokens.

## 5) Examples (reference)

- See **@docs/COD-examples.md** for C-O-D patterns.
