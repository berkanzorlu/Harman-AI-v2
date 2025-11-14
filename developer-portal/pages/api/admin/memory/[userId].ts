import type { NextApiRequest, NextApiResponse } from 'next';
import { prisma } from '../../../../lib/prisma';

export default async function handler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method !== 'GET') return res.status(405).end();
  const { userId } = req.query;
  const chunks = await prisma.memoryChunk.findMany({ where: { userId: userId as string }, orderBy: { createdAt: 'desc' }, take: 100 });
  res.json({ chunks });
}
