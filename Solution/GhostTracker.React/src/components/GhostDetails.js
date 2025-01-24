import { useEffect, useState } from 'react';
import './GhostDetails.css';

function GhostDetails({ onClose, id }) {
    const [details, setDetails] = useState(undefined);

    useEffect(() => {
        if (!id) return;

        const fetchDetails = async () => {
            try {
                const response = await fetch(`bff/ghosts/${id}`);
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const detailsJson = await response.json();
                setDetails(detailsJson);
            } catch (error) {
                console.error('Fetch error:', error);
            }
        };

        fetchDetails();
    }, [id]);

    if (!details) {
        return null;
    }

    return (
        <div className="modal-overlay">
            <div className="modal">
                <button className="modal-close" onClick={onClose}>×</button>
                <div className="modal-content">
                    <h2>{details.name}</h2>
                    <div>Type: {details.type}</div>
                    <div>Age: {details.age}</div>
                    <div>Date of dead: {details.dateOfDead.substring(0, 10)}</div>
                    <div>Original haunt location: {details.hauntLocation}</div>
                    <div>Appearance: {details.appearance}</div>
                    <div>Danger level: {details.dangerLevel}</div>
                    <div>Abilities: {details.abilities}</div>
                </div>
            </div>
        </div>
    );
}

export default GhostDetails;
