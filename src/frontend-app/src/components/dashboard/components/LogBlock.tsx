


const LogBlock = ({ log }: { log: string[] }) => {
    return (<div>
        <label className="block text-sm font-medium mb-1">Log</label>
        <textarea
            readOnly
            value={log.join("\n")}
            className="w-full h-48 border rounded p-2 font-mono text-xs" />
    </div>);
}
export default LogBlock;