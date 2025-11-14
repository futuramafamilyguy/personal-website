import { useState } from "react";

import { Track } from "../../types/Radio";
import Booth from "./Booth/Booth";

interface QstFmProps {
  playlist: Track[];
}

const QstFmContainer: React.FC<QstFmProps> = ({ playlist }) => {
  const [current, setCurrent] = useState(0);
  const [isPlaying, setIsPlaying] = useState(false);

  const handleEnded = () => {
    setCurrent((prev) => (prev + 1) % playlist.length);
  };

  const handlePlayPause = (playing: boolean) => {
    setIsPlaying(playing);
  };

  return (
    <Booth
      key={playlist[current].url}
      title={playlist[current].title}
      artist={playlist[current].artist}
      url={playlist[current].url}
      original={playlist[current].original}
      onEnded={handleEnded}
      autoPlay={isPlaying}
      onPlayPause={handlePlayPause}
    />
  );
};

export default QstFmContainer;
