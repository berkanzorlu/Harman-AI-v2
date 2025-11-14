import type { NextApiRequest, NextApiResponse } from 'next';
import { prisma } from '../../../lib/prisma';

export default async function handler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method !== 'GET') return res.status(405).end();
  const { id } = req.query;
  const license = await prisma.license.findUnique({ where: { id: id as string }, include: { activations: true } });
  if (!license) return res.status(404).json({ error: 'Not found' });
  res.json({ license });
}
