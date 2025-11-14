import Link from 'next/link';

export default function Home() {
  return (
    <main>
      <h1 className="text-3xl font-bold">Harman AI Developer Portal</h1>
      <p className="mt-2 text-slate-300">Manage licenses, activations, and memory.</p>
      <div className="mt-8 grid grid-cols-1 md:grid-cols-2 gap-4">
        <div className="card">
          <h2 className="text-xl font-semibold">License Console</h2>
          <p>Track activations, generate license keys, and revoke compromised devices.</p>
        </div>
        <div className="card">
          <h2 className="text-xl font-semibold">Memory Explorer</h2>
          <p>Search user memories, filter by context, and export JSON snapshots.</p>
        </div>
      </div>
      <Link className="inline-block mt-6 text-blue-400" href="/dashboard">Go to dashboard →</Link>
    </main>
  );
}
