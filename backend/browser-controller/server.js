import express from 'express';
import morgan from 'morgan';
import { RateLimiterMemory } from 'rate-limiter-flexible';
import { BrowserController } from './browser.js';

const app = express();
const controller = new BrowserController();
const limiter = new RateLimiterMemory({ points: 30, duration: 60 });

app.use(express.json({ limit: '1mb' }));
app.use(morgan('dev'));

app.use(async (req, res, next) => {
  const token = req.headers['x-license-token'];
  if (!token) {
    return res.status(401).json({ success: false, error: 'Missing license token' });
  }
  try {
    await limiter.consume(token);
    next();
  } catch {
    res.status(429).json({ success: false, error: 'Rate limit exceeded' });
  }
});

const wrap = handler => async (req, res) => {
  try {
    const data = await handler(req.body);
    res.json({ success: true, data });
  } catch (err) {
    res.status(500).json({ success: false, error: err.message });
  }
};

app.post('/open', wrap(body => controller.open(body.url)));
app.post('/click', wrap(body => controller.click(body.selector)));
app.post('/fill', wrap(body => controller.fill(body.selector, body.text)));
app.post('/getText', wrap(body => controller.getText(body.selector)));
app.get('/getLinks', wrap(() => controller.getLinks()));
app.post('/scroll', wrap(body => controller.scroll(body.x, body.y)));
app.get('/screenshot', wrap(() => controller.screenshot()));

app.listen(4000, () => console.log('Browser controller running on 4000'));
