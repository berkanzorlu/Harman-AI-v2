'use client';

import { useState } from 'react';

const sampleMemory = [
  { id: '1', role: 'user', content: 'Open browser', createdAt: '2024-03-01' },
  { id: '2', role: 'assistant', content: 'Opening Edge now', createdAt: '2024-03-01' }
];

export default function Dashboard() {
  const [query, setQuery] = useState('');
  const filtered = sampleMemory.filter(m => m.content.toLowerCase().includes(query.toLowerCase()));

  return (
    <main>
      <h1 className="text-2xl font-bold mb-4">Admin Dashboard</h1>
      <section className="card">
        <h2 className="font-semibold">License Generator</h2>
        <form className="grid grid-cols-1 md:grid-cols-3 gap-2 mt-2">
          <input placeholder="User email" className="p-2 rounded bg-slate-800" />
          <input placeholder="Max activations" className="p-2 rounded bg-slate-800" />
          <button className="bg-blue-500 rounded px-4 py-2">Generate</button>
        </form>
      </section>
      <section className="card">
        <h2 className="font-semibold">Memory Search</h2>
        <input value={query} onChange={e => setQuery(e.target.value)} className="p-2 rounded bg-slate-800 w-full" placeholder="Search memories" />
        <div className="mt-4 space-y-2 max-h-64 overflow-y-auto">
          {filtered.map(chunk => (
            <div key={chunk.id} className="border border-slate-800 p-2 rounded">
              <div className="text-xs text-slate-400">{chunk.role} • {chunk.createdAt}</div>
              <div>{chunk.content}</div>
            </div>
          ))}
        </div>
      </section>
    </main>
  );
}
