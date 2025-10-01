# 🚀 Emoji Stock Exchange Workshop - Getting Started

## Setting up

Clone the repository

`git clone https://github.com/kraftlauget/emoji-trader-bot-workshop.git`

Ensure you have .NET SDK 8.0 or later available.

Do a sanity check by running the project.

On the command line: `dotnet run`

In Visual Studio or VS Code: Run the project using the IDE's run/debug features

You should see "Bot initialized successfully" and "Bot is ready for trading operations" in the console.

Now you are ready to start!

## What You're Building

You'll create a trading bot that autonomously buys and sells emoji stocks (🦄, 💎, ❤️, 🍌, 🍾, 💻) to maximize profit. Your bot will compete against others in real-time on a live exchange.

## How the Exchange Works

- **Starting Position**: $10,000 cash + 100 shares of each emoji symbol (participants only)
- **Goal**: Maximize your total equity (cash + stock value)
- **Trading**: Submit buy/sell limit orders via REST API
- **Competition**: Real-time leaderboard shows P&L rankings
- **Rate Limit**: 50 requests/second per team

### Order Book Mechanics

The exchange operates as a **central limit order book** with price-time priority:

- **Bids (Buy Orders)**: Sorted highest price first, then by time
- **Asks (Sell Orders)**: Sorted lowest price first, then by time
- **Order Matching**: Incoming orders match against best opposite price
- **Spread**: Gap between best bid and best ask (e.g., bid $10.00, ask $10.50)

**Key Insight**: You buy at the **ask price**, sell at the **bid price**

### Order Book Example

```
Symbol: 🦄
Asks (Sellers):  $10.75 (50 shares)
                 $10.50 (100 shares)  ← Best Ask
                 ─────────────────────
Bids (Buyers):   $10.00 (75 shares)   ← Best Bid
                 $9.75  (200 shares)
```

- To **buy immediately**: Submit market buy → executes at $10.50
- To **sell immediately**: Submit market sell → executes at $10.00
- **Spread cost**: $0.50 per share ($10.50 - $10.00)

## Resources

- **API Documentation**: [Swagger UI](https://emoji-stock-exchange-2-h52e5.ondigitalocean.app/swagger) - Interactive API explorer
- **API Spec**: `openapi.yaml` - In this repository you can find the spec as well.
- **Exchange URL**: `https://emoji-stock-exchange-2-h52e5.ondigitalocean.app`
- **Leaderboard GUI**: Check `/dashboard` route for a GUI with the rankings
- **Leaderboard JSON**: Check `/leaderboard` endpoint for rankings

## Workshop Tasks

### 1. 🏷️ **Team Setup** (10 min)

- Decide on a creative team name for your trading bot
- Update `Program.cs` with your chosen team name
- Make it memorable - you'll see it on the leaderboard!

### 2. 🔧 **Build & Run Verification** (10 min)

- Verify you can build the project successfully
- Run the application and confirm it connects to the exchange
- Check that your team appears ready to trade
- **Troubleshoot any setup issues now before moving forward**

### 3. 📚 **Study the Market** (20 min)

- Read and understand the rest of this WORKSHOP-INTRO document
- Pay special attention to the **Order Book Mechanics** and **Four Bot Strategies**
- Explore the API endpoints using the Swagger UI
- Check the current leaderboard to see what you're up against

### 4. 🎓 **Create Your Trading Expert** (15 min)

Before you start coding, create a specialized sub-agent to help explain trading concepts:

- Use Claude Code's `/agents` command to create a new sub-agent
- **Learn about sub-agents**: [Claude Code Sub-Agents Documentation](https://docs.claude.com/en/docs/claude-code/sub-agents)
- **Example prompt for creating your agent**:

```
Create a sub-agent called "trading-expert" that specializes in stock trading concepts.
This agent should:
- Explain complex trading strategies in simple terms (ELI5 style)
- Help break down order book mechanics, spreads, and market dynamics
- Provide quick answers about trading terminology
- Give practical advice for implementing trading algorithms
- Be encouraging and supportive for beginners

Make it focused specifically on helping workshop participants understand trading concepts.
```

This trading expert will be your go-to helper during implementation when you need concepts explained!

### 5. 📋 **Plan with Claude Code** (30 min)

**This is the key step - don't rush it!**

- Open Claude Code and tell it: _"Help me create a plan for building a trading bot for a 3-hour workshop"_
- Share this WORKSHOP-INTRO document with Claude Code for context
- **Discuss and refine the plan until you're completely happy with it**
- Choose one of the four bot strategies that appeals to you
- Break down the implementation into manageable steps
- **Break the implementation into stages, ensuring you can validate progress after each stage**
- Consider: What's realistic to build in ~2 hours of coding time?

### 6. 🚀 **Implement Step by Step** (90-120 min)

- Work through your plan systematically with Claude Code's help
- **Use your trading expert sub-agent** when you need concepts explained
- **Understand each piece of code before moving to the next step**
- Guide Claude Code to implement exactly what you want, not what it thinks is best
- Test frequently - make sure each component works before adding complexity
- Start simple and add sophistication gradually
- **Remember: A working simple bot beats a broken complex one**

### 7. ⚔️ **Trade Wars!** (30 min)

- Deploy your bot and watch it compete on the leaderboard
- Monitor its performance and make real-time adjustments
- Learn from other teams' strategies by watching market behavior
- Celebrate your success (or learn from interesting failures!)

---

## Key Trading Concepts

**Strategy Fundamentals**:

- **Profitable Buy**: Place bid below current best ask
- **Profitable Sell**: Place ask above current best bid
- **Avoid Market Orders**: They execute immediately at worst price
- **Example**: If spread is $10.00-$10.50, place bid at $10.24, ask at $10.26 (quoting around the mid)

**Market Order Usage**:

- Use market orders only when: (1) spread ≤ 1 tick and you need certainty, (2) you're exiting risk, or (3) you detect obviously stale quotes
- Otherwise you pay the spread - use limit orders to control your execution price

**Industrial Order Detection**:
Look for anomalies in order book depth:

- Compute rolling median depth per price level
- Flag when top-3 levels exceed typical size by 3x or more
- Consider orderCount = 1 with large quantity as indicator
- Monitor for sudden depth changes that create trading opportunities

**Why Industrial Orders = Profit Opportunities**:

- Large orders can create temporary price imbalances
- May offer favorable trading opportunities, but many bots compete
- Only participate when your expected edge exceeds adverse selection risk
- **Speed and smart positioning matter**

## 🎮 Four Bot Strategies to Try

### 1. Market Maker (The Shopkeeper)

Think of your bot like a shop: you buy a little cheaper, sell a little higher.

- Place buy orders just below the current price, and sell orders just above it
- Profit comes from earning the small difference (the "spread")
- Risk: if prices suddenly move, you may be stuck with too much of one stock

👉 Good for steady small profits if your bot keeps balance.

### 2. Sniper (The Opportunist)

Your bot just sits and watches the market.

- When a huge order ("industrial order") shows up, jump in quickly to trade against it
- Example: if a giant buy order appears, you sell to it and try to buy back cheaper later
- Risk: if you're too slow, others get there first, or the price moves against you

👉 Good for fast-action coders who want to race others.

### 3. Trend Follower (The Surfer)

Imagine you're riding waves.

- If the price is rising, your bot buys in, hoping to ride it up a bit more
- If the price is falling, your bot sells, riding it down
- Risk: if the wave ends suddenly, you lose money

👉 Good for bots that "guess direction" from simple rules, like last 3 prices going up = buy.

### 4. Momentum Reverter (The Contrarian)

When price moves too far too fast, bet it will snap back.

- If 🦄 jumps from $10 to $10.50 quickly, place sell orders expecting it to fall back
- If it drops to $9.50, place buy orders expecting a bounce
- Risk: trends can continue longer than expected

👉 Good for bots that detect "overshoots" and bet on corrections.

**📌 Tip for participants**: Start simple. Even a bot that just buys a little below and sells a little above will teach you how orders, fills, and portfolios work. Then you can add the "sniper" or "trend" logic later if you want more excitement.

## Success Tips

### Trading Strategy

- **Understand the spread** - Always know bid vs ask prices
- **Never market buy/sell** - Use limit orders to control price
- **Target the middle** - Place orders between bid and ask
- **Skew quotes for inventory** - If long 200 🍌, lower your bid and ask to sell faster; if short, do the reverse
- **Widen spreads during volatility** - When market moves quickly or opposite side thins out
- **Watch position limits** - Don't overexpose to one symbol

### Industrial Order Strategy

- **Monitor order book depth** - Look for quantity > 150
- **React fast** - Other teams compete for same opportunities
- **Calculate profit first** - Ensure spread > transaction costs
- **Time your checks** - New opportunities appear every ~5 minutes

### Development Tips

- **Start with manual testing** - Use Swagger UI first
- **Test your math** - Verify profit calculations
- **Check the leaderboard** - Track performance vs others
- **Handle rate limits** - 50 req/sec max

### Common Mistakes to Avoid

- ❌ Buying at ask, selling at bid (guarantees loss)
- ❌ Using market orders (worst execution price)
- ❌ Ignoring position limits (overconcentration risk)
- ❌ Not validating profit before placing orders

## Available Symbols

- 🦄 Unicorn
- 💎 Diamond
- ❤️ Heart
- 🍌 Banana
- 🍾 Champagne
- 💻 Computer

## API Quick Reference

### Place Limit Order

```bash
curl -X POST https://emoji-stock-exchange-2-h52e5.ondigitalocean.app/v1/orders \
  -H "Content-Type: application/json" \
  -H "X-Team-Id: YOUR_TEAM_ID" \
  -H "X-Api-Key: YOUR_API_KEY" \
  -d '{
    "side": "BUY",
    "symbol": "🦄",
    "quantity": 50,
    "limitPrice": 10.25,
    "timeInForce": "GTC"
  }'
```

### Check Order Book

```bash
# See all current orders (requires authentication)
curl "https://emoji-stock-exchange-2-h52e5.ondigitalocean.app/v1/orderbook?symbol=🦄&depth=10" \
  -H "X-Team-Id: YOUR_TEAM_ID" \
  -H "X-Api-Key: YOUR_API_KEY"

# Response shows bids (buyers) and asks (sellers)
{
  "bids": [{"price": 10.00, "quantity": 75, "orderCount": 1}],
  "asks": [{"price": 10.50, "quantity": 100, "orderCount": 1}]
}
```

### Check Your Portfolio

```bash
curl https://emoji-stock-exchange-2-h52e5.ondigitalocean.app/v1/portfolio/YOUR_TEAM_ID \
  -H "X-Team-Id: YOUR_TEAM_ID" \
  -H "X-Api-Key: YOUR_API_KEY"

# Response shows cash, positions, and total equity
{
  "teamId": "my-team-123",
  "cash": 9500.0,
  "positions": {
    "🦄": 10,
    "💎": -5
  },
  "equity": 9750.0,
  "timestamp": "2025-01-01T12:00:00Z"
}
```

**Key fields:**
- `cash`: Available cash for buying
- `positions`: Shares owned (positive) or short (negative) per symbol
- `equity`: Total value (cash + positions at current market prices)

**When to check portfolio:**
- Before placing orders (verify sufficient cash/shares)
- Monitor position limits (avoid overconcentration in one symbol)
- Calculate current equity vs starting equity ($10,000 + initial positions)

### Monitor Your Orders

```bash
curl https://emoji-stock-exchange-2-h52e5.ondigitalocean.app/v1/orders \
  -H "X-Team-Id: YOUR_TEAM_ID" \
  -H "X-Api-Key: YOUR_API_KEY"
```

### Cancel Stale Orders

```bash
# Cancel an order by its order ID
curl -X DELETE https://emoji-stock-exchange-2-h52e5.ondigitalocean.app/v1/orders/ORDER_ID \
  -H "X-Team-Id: YOUR_TEAM_ID" \
  -H "X-Api-Key: YOUR_API_KEY"

# Response on success
{"status": "cancelled"}
```

**When to cancel orders:**
- Price has moved away from your order (no longer competitive)
- Order has been sitting unfilled for too long
- You need to adjust position sizing or strategy
- Market conditions have changed significantly

### Watch for Fills (Executions)

```bash
# Get all your trade executions
curl https://emoji-stock-exchange-2-h52e5.ondigitalocean.app/v1/fills \
  -H "X-Team-Id: YOUR_TEAM_ID" \
  -H "X-Api-Key: YOUR_API_KEY"

# Get only new fills since last check (pagination)
curl "https://emoji-stock-exchange-2-h52e5.ondigitalocean.app/v1/fills?since=12345" \
  -H "X-Team-Id: YOUR_TEAM_ID" \
  -H "X-Api-Key: YOUR_API_KEY"
```

**What are fills?**
- Each **fill** = one individual trade execution
- One order can generate multiple fills (partial executions)
- Fills show the exact price and quantity of each trade

**When to use fills vs orders:**
- **Orders** (`/v1/orders`): Check order status, manage working orders, see what's still open
- **Fills** (`/v1/fills`): Calculate P&L, track position changes, audit execution prices, see what actually traded

**Why monitor fills:**
- Update your position tracking after each execution
- Calculate realized profit/loss from completed trades
- React quickly to executions (e.g., place opposite orders for market making)
- Use `since` parameter to poll efficiently for new fills only

**Ready to trade? Let's build your bot! 📈**
