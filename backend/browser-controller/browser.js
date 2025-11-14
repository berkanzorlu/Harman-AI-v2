import { chromium } from 'playwright';

export class BrowserController {
  constructor() {
    this.browserPromise = chromium.launch({ headless: true });
  }

  async _getPage() {
    if (!this.page) {
      const browser = await this.browserPromise;
      const context = await browser.newContext();
      this.page = await context.newPage();
    }
    return this.page;
  }

  async open(url) {
    const page = await this._getPage();
    await page.goto(url);
    return { url: page.url() };
  }

  async click(selector) {
    const page = await this._getPage();
    await page.click(selector);
    return { selector };
  }

  async fill(selector, text) {
    const page = await this._getPage();
    await page.fill(selector, text);
    return { selector, text };
  }

  async getText(selector) {
    const page = await this._getPage();
    const text = await page.textContent(selector);
    return { text };
  }

  async getLinks() {
    const page = await this._getPage();
    const links = await page.$$eval('a', nodes => nodes.map(n => ({ href: n.href, text: n.textContent })));
    return { links };
  }

  async scroll(x, y) {
    const page = await this._getPage();
    await page.mouse.wheel(x, y);
    return { x, y };
  }

  async screenshot() {
    const page = await this._getPage();
    const buffer = await page.screenshot();
    return { base64: buffer.toString('base64') };
  }
}
