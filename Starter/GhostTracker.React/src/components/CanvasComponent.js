import React, { useRef, useEffect, useState } from 'react';
import GhostDetails from './GhostDetails';

const GHOST_SIZE = 50;
const GHOST_SIZE_HALF = GHOST_SIZE / 2;

function CanvasComponent() {
  const intervalId = useRef(null);
  const canvasRef = useRef(null);
  const [imageMemory, setImageMemory] = useState(undefined);
  const [ghostSummary, setGhostSummary] = useState(undefined);

  const [isOpen, setIsOpen] = useState(false);
  const [selectedId, setSelectedId] = useState(undefined);
  
  // Load images.
  useEffect(() => {
    intervalId.current = setInterval(() => {
      retrieveGhosts()
    }, 5000);

    retrieveGhosts()
    const bg = new Image();
    bg.src = './public/street_map.jpeg';
    bg.onload = () => {
      console.log('Background loaded');
    };

    const ghost1 = new Image();
    ghost1.src = './public/ghost1.png';
    ghost1.onload = () => {
      console.log('Ghost 1 loaded');
    };

    setImageMemory({
      bg,
      ghost1
    });
  }, []);

  useEffect(() => {
    return () => {
      clearInterval(intervalId.current);
    }
  }, []);

  useEffect(() => {renderCanvas()}, [ghostSummary]);

  useEffect(() => {
    renderCanvas();
  }, [imageMemory]);

  const retrieveGhosts = async () => {
    const response = await fetch("bff/ghosts/summary");
    console.log(response);

    if (response.status != 200) {
      setGhostSummary({ ghosts: [] });
      return;
    }

    const ghostSummaryTemp = await response.json();
    console.log(ghostSummaryTemp);

    setGhostSummary(ghostSummaryTemp);
  }
  
  const renderCanvas = async () => {
    if (!imageMemory 
      || !imageMemory.bg 
      || !imageMemory.bg.complete 
      || !imageMemory.ghost1.complete) return;
    
    const canvas = canvasRef.current;
    const ctx = canvas.getContext('2d');

    ctx.drawImage(imageMemory.bg, 0, 0);

    ghostSummary.ghosts.forEach(ghost => {
      ghost.line.forEach(coord => {
        ctx.beginPath();
        ctx.arc(coord.x, coord.y, 6, 0, 2 * Math.PI, false);
        ctx.fillStyle = '#AF2655';
        ctx.fill();
        ctx.lineWidth = 2;
        ctx.strokeStyle = '#860A35';
        ctx.stroke();
      })

      ctx.drawImage(imageMemory.ghost1, ghost.x - GHOST_SIZE_HALF, ghost.y - GHOST_SIZE_HALF, GHOST_SIZE, GHOST_SIZE);
    });

  };

  const handleClick = (event) => {
    const canvas = canvasRef.current;
    const rect = canvas.getBoundingClientRect();
    const x = event.clientX - rect.left;
    const y = event.clientY - rect.top;

    ghostSummary.ghosts.forEach(ghost => {
      if (calculateDistance(x, y, ghost.x, ghost.y) < 60) {
        setSelectedId(ghost.id);
        setIsOpen(true);
      }
    });
  }

  const handleClose = () => {
    setIsOpen(false);
  }

  const calculateDistance = (x1, y1, x2, y2) => {
    return Math.abs(x1 - x2) + Math.abs(y1 - y2);
  }

  return (
    <>
      { ghostSummary && ghostSummary.ghosts.length == 0 && <div>No active ghosts found</div>}
      { imageMemory && imageMemory.bg && !imageMemory.bg.complete && <div>Loading map...</div>}
      <div>
        <canvas ref={canvasRef} width={2048} height={1536} onClick={handleClick} />
        {isOpen && 
          <GhostDetails onClose={() => handleClose()} id={selectedId}></GhostDetails> }
      </div>
    </>
  );
}

export default CanvasComponent;
