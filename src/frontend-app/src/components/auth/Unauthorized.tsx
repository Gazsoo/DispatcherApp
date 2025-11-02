import React from "react";
import { Link } from "react-router-dom";

export default function Unauthorized() {
    return (
        <div className="p-6">
            <h2 className="text-xl font-semibold mb-2">Access denied</h2>
            <p className="text-sm text-gray-600 mb-4">You do not have the necessary permissions to view this page.</p>
            <Link to="/dashboard" className="text-sm text-blue-600">Return to dashboard</Link>
        </div>
    );
}
