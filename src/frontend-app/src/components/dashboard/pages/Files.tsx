import { Outlet, useParams } from "react-router-dom";

export default function Files() {
    const { fileId } = useParams();

    // If viewing a specific file, only show the Outlet
    if (fileId) {
        return <Outlet />;
    }

    // Otherwise show the files list
    return (
        <div>
            <h1 className="text-3xl font-bold mb-6">Files</h1>
            {/* Files list */}
        </div>
    );
}