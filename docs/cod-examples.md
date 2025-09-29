# COD Loop Examples – READ THESE CAREFULLY

## Example 1: Authentication Feature

**Developer**: "Add JWT authentication to the user endpoints"

**Assistant** [CLARIFY]: "I need to understand your authentication requirements:

- Are user accounts already in the database?
- Do you need refresh tokens or just access tokens?
- Which endpoints need protection?
- Any specific JWT expiry time?"

**Developer**: "Users exist in db, just access tokens, protect all /api/users/\* endpoints, 24h expiry"

**Assistant** [OFFER]: "Here are implementation approaches:

### Option A – Use System.IdentityModel.Tokens.Jwt

Pros: Battle-tested, full JWT spec support
Cons: Additional dependency

### Option B – Use jwt.net

Pros: Lightweight, already in project
Cons: Less feature-rich

**Developer** [DECIDE]: "Use Option B since we already have jwt.net"

## Example 2: Database Query Optimization

**Developer**: "The product search is slow, can you optimize it?"

**Assistant** [CLARIFY]: "Let me understand the issue:

- Which specific endpoint/query is slow?
- Do you have any performance metrics?
- Is this happening in dev or production?
- Any recent changes to the data volume?"

**Developer**: "GET /api/products/search, takes 3+ seconds, production only, 100k products now"

**Assistant** [OFFER]: "I see the issue. Here are optimization approaches:

### Option A – Add database indexes

Pros: Simple, immediate improvement
Cons: Increases write time slightly

### Option B – Implement caching layer

Pros: Dramatic speed improvement for repeat queries
Cons: Cache invalidation complexity

### Option C – Paginate results + indexes

Pros: Consistent performance
Cons: Requires frontend changes

I suggest starting with Option A as it's lowest risk."

**Developer** [DECIDE]: "Let's do A first, then evaluate if we need B"
