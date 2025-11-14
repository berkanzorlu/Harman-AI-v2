import type { NextApiRequest, NextApiResponse } from 'next';
import { prisma } from '../../../lib/prisma';

export default async function handler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method !== 'POST') return res.status(405).end();
  const { licenseId } = req.body;
  await prisma.license.update({ where: { id: licenseId }, data: { revokedAt: new Date() } });
  res.json({ status: 'revoked' });
}
