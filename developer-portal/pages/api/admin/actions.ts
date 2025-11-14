import type { NextApiRequest, NextApiResponse } from 'next';
import { prisma } from '../../../lib/prisma';

export default async function handler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method !== 'GET') return res.status(405).end();
  const actions = await prisma.actionHistory.findMany({ orderBy: { createdAt: 'desc' }, take: 100 });
  res.json({ actions });
}
