import { useParams } from "react-router";

export default function FileDetails() {
    const { fileId } = useParams();

    return (
        <div>
            <h1 className="text-3xl font-bold mb-6">File Details for {fileId}</h1>
            {/* File details for fileId: {fileId} */}
        </div>
    );
}