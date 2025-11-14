import type { NextApiRequest, NextApiResponse } from 'next';
import jwt from 'jsonwebtoken';

export default async function handler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method !== 'POST') return res.status(405).end();
  try {
    const { token } = req.body;
    const decoded = jwt.verify(token, process.env.JWT_PUBLIC_KEY!, { algorithms: ['RS256'] });
    res.json({ valid: true, decoded });
  } catch (err) {
    res.status(401).json({ valid: false, error: (err as Error).message });
  }
}
