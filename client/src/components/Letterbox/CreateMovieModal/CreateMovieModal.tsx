import { AxiosResponse } from "axios";
import React, { FormEvent, useEffect, useState } from "react";
import ReactDom from "react-dom";

import {
  debouncedFetchCinemas,
  makeDebouncedRequest,
} from "../../../api/debouncedFetch";
import {
  createMovie,
  CreateMovieRequest,
  deleteMovie,
  getPresignedImageUrl,
  updateMovie,
  UpdateMovieRequest,
  uploadImageToPresignedUrl,
} from "../../../api/movies";
import { useYear } from "../../../contexts/YearContext";
import Cinema from "../../../types/Cinema";
import Movie from "../../../types/Movie";
import styles from "./CreateMovieModal.module.css";

interface CreateMovieModalProps {
  isOpen: boolean;
  onClose: () => void;
  movie: Movie | null;
  setTrigger: () => void;
}

const CreateMovieModal: React.FC<CreateMovieModalProps> = ({
  isOpen,
  onClose,
  movie,
  setTrigger,
}) => {
  const [name, setName] = useState("");
  const [releaseYear, setReleaseYear] = useState("");
  const [imageFile, setImageFile] = useState<File | null>(null);
  const [imageUrl, setImageUrl] = useState("");
  const [imageObjectKey, setImageObjectKey] = useState("");
  const [month, setMonth] = useState("");
  const [cinemaId, setCinemaId] = useState("");
  const [zinger, setZinger] = useState("");
  const [alias, setAlias] = useState("");
  const [isNominated, setIsNominated] = useState(false);
  const [isKino, setIsKino] = useState(false);
  const [isRetro, setIsRetro] = useState(false);
  const [altImageFile, setAltImageFile] = useState<File | null>(null);
  const [altImageUrl, setAltImageUrl] = useState("");
  const [altImageObjectKey, setAltImageObjectKey] = useState("");

  const [result, setResult] = useState("");

  const [cinemas, setCinemas] = useState<Cinema[]>([]);
  const year = useYear();

  useEffect(() => {
    setResult("");
    setName(movie ? movie.name : "");
    setReleaseYear(movie ? movie.releaseYear.toString() : "");
    setImageFile(null);
    setImageUrl(movie && movie.imageUrl ? movie.imageUrl : "");
    setImageObjectKey(
      movie && movie.imageObjectKey ? movie.imageObjectKey : ""
    );
    setMonth(movie ? movie.month.toString() : "");
    setCinemaId(movie ? movie.cinema.id : "");
    setZinger(movie && movie.zinger ? movie.zinger : "");
    setAlias(movie && movie.alias ? movie.alias : "");
    setIsNominated(movie ? movie.isNominated : false);
    setIsKino(movie ? movie.isKino : false);
    setIsRetro(movie ? movie.isRetro : false);
    setAltImageFile(null);
    setAltImageUrl(movie && movie.altImageUrl ? movie.altImageUrl : "");
    setAltImageObjectKey(
      movie && movie.altImageObjectKey ? movie.altImageObjectKey : ""
    );
  }, [isOpen]);

  useEffect(() => {
    const fetchCinemas = () => {
      makeDebouncedRequest(debouncedFetchCinemas, {
        url: "/cinemas",
      })
        .then((response: AxiosResponse<Cinema[]>) => {
          setCinemas(response.data);
        })
        .catch((error: any) => {
          console.error("Error fetching cinemas:", error);
        });
    };

    fetchCinemas();
  }, []);

  const orchestrateCreateMovie = async () => {
    try {
      const data: CreateMovieRequest = {
        year: year!,
        name: name,
        month: parseInt(month, 10),
        releaseYear: parseInt(releaseYear, 10),
        cinemaId: cinemaId,
        zinger: zinger ? zinger : null,
        alias: alias ? alias : null,
        isNominated: isNominated,
        isKino: isKino,
        isRetro: isRetro,
      };
      const newMovie = await createMovie(data);

      if (imageFile) {
        const extension = imageFile.name.split(".").pop()?.toLowerCase();
        const { presignedUploadUrl } = await getPresignedImageUrl({
          id: newMovie.id,
          extension: extension!,
          isAlt: false,
        });

        await uploadImageToPresignedUrl(presignedUploadUrl, imageFile);
      }
      if (altImageFile) {
        const extension = altImageFile.name.split(".").pop()?.toLowerCase();
        const { presignedUploadUrl } = await getPresignedImageUrl({
          id: newMovie.id,
          extension: extension!,
          isAlt: true,
        });

        await uploadImageToPresignedUrl(presignedUploadUrl, altImageFile);
      }

      setTrigger();
      onClose();
    } catch (error: any) {
      console.error("error creating movie:", error);
      setResult("error creating movie");
    }
  };

  const orchestrateUpdateMovie = async () => {
    try {
      const data: UpdateMovieRequest = {
        id: movie!.id,
        name: name,
        year: year!,
        month: parseInt(month, 10),
        releaseYear: parseInt(releaseYear, 10),
        cinemaId: cinemaId,
        zinger: zinger ? zinger : null,
        alias: alias ? alias : null,
        isNominated: isNominated,
        isKino: isKino,
        isRetro: isRetro,
        imageUrl: imageObjectKey ? imageUrl : null, // use object key to determine image presence due to default image
        imageObjectKey: imageObjectKey ? imageObjectKey : null,
        altImageUrl: altImageObjectKey ? altImageUrl : null,
        altImageObjectKey: altImageObjectKey ? altImageObjectKey : null,
      };
      const updatedMovie = await updateMovie(data);

      if (imageFile) {
        const extension = imageFile.name.split(".").pop()?.toLowerCase();
        const { presignedUploadUrl } = await getPresignedImageUrl({
          id: updatedMovie.id,
          extension: extension!,
          isAlt: false,
        });

        await uploadImageToPresignedUrl(presignedUploadUrl, imageFile);
      }
      if (altImageFile) {
        const extension = altImageFile.name.split(".").pop()?.toLowerCase();
        const { presignedUploadUrl } = await getPresignedImageUrl({
          id: updatedMovie.id,
          extension: extension!,
          isAlt: true,
        });

        await uploadImageToPresignedUrl(presignedUploadUrl, altImageFile);
      }

      setTrigger();
      onClose();
    } catch (error: any) {
      console.error("error updating movie:", error);
      setResult("error updating movie");
    }
  };

  const orchestrateDeleteMovie = async () => {
    try {
      await deleteMovie(movie!.id);

      setTrigger();
      onClose();
    } catch (error: any) {
      console.error("error deleting movie:", error);
      setResult("error deleting movie");
    }
  };

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault();
    if (movie) {
      orchestrateUpdateMovie();
    } else {
      orchestrateCreateMovie();
    }
  };

  const handleDelete = (event: FormEvent) => {
    event.preventDefault();
    orchestrateDeleteMovie();
  };

  if (!isOpen) return null;

  return ReactDom.createPortal(
    <>
      <div className={styles.overlay} onClick={onClose}></div>
      <div className={styles.modal}>
        <div className={styles.textContainer}>
          {movie ? (
            <h5 className={styles.title}>update movie</h5>
          ) : (
            <h5 className={styles.title}>add new movie</h5>
          )}
          <form onSubmit={handleSubmit}>
            <div className={styles.formFields}>
              <div className={styles.formGroup}>
                <label>name</label>
                <input
                  type="text"
                  id="name"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  required
                  className={styles.inputField}
                ></input>
              </div>
              <div className={styles.formGroup}>
                <label>release year</label>
                <input
                  type="text"
                  id="release_year"
                  value={releaseYear}
                  onChange={(e) => setReleaseYear(e.target.value)}
                  required
                ></input>
              </div>
              <div className={styles.formGroup}>
                <label>image</label>
                <input
                  type="file"
                  onChange={(e) =>
                    setImageFile(e.target.files ? e.target.files[0] : null)
                  }
                />
              </div>
              <div className={styles.formGroup}>
                <label>alt image</label>
                <input
                  type="file"
                  onChange={(e) =>
                    setAltImageFile(e.target.files ? e.target.files[0] : null)
                  }
                />
              </div>
              <div className={styles.formGroup}>
                <label>month</label>
                <input
                  type="text"
                  id="month"
                  value={month}
                  onChange={(e) => setMonth(e.target.value)}
                ></input>
              </div>
              <div className={styles.formGroup}>
                <label>cinema</label>
                <input
                  type="text"
                  id="cinema_id"
                  value={
                    cinemaId !== ""
                      ? cinemas.filter((cinema) => cinema.id === cinemaId)[0]
                          .name
                      : ""
                  }
                  onChange={(e) => setCinemaId(e.target.value)}
                  className={styles.hiddenInput}
                ></input>
                <select
                  id="cinema_id"
                  value={cinemaId}
                  onChange={(e) => setCinemaId(e.target.value)}
                  required
                >
                  <option value="">
                    {cinemaId !== ""
                      ? cinemas.filter((cinema) => cinema.id === cinemaId)[0]
                          .name
                      : "select a cinema"}
                  </option>
                  {cinemas.map((cinema) => (
                    <option key={cinema.id} value={cinema.id}>
                      {cinema.name}
                    </option>
                  ))}
                </select>
              </div>
              <div className={styles.formGroup}>
                <label>nominated</label>
                <div>
                  <input
                    type="checkbox"
                    id="nominated"
                    checked={isNominated}
                    onChange={() => setIsNominated(!isNominated)}
                  />
                </div>
              </div>
              <div className={styles.formGroup}>
                <label>KINO</label>
                <div>
                  <input
                    type="checkbox"
                    id="kino"
                    checked={isKino}
                    onChange={() => setIsKino(!isKino)}
                  />
                </div>
              </div>
              <div className={styles.formGroup}>
                <label>retro</label>
                <div>
                  <input
                    type="checkbox"
                    id="retro"
                    checked={isRetro}
                    onChange={() => setIsRetro(!isRetro)}
                  />
                </div>
              </div>
              <div className={styles.formGroup}>
                <label>alias</label>
                <input
                  type="text"
                  id="alias"
                  value={alias}
                  onChange={(e) => setAlias(e.target.value)}
                ></input>
              </div>
              <div className={styles.formGroup}>
                <label>zinger</label>
                <input
                  type="text"
                  id="zinger"
                  value={zinger}
                  onChange={(e) => setZinger(e.target.value)}
                ></input>
              </div>
            </div>
            <div className={styles.buttonContainer}>
              <div>
                <button className={styles.button} type="submit">
                  submit
                </button>
                <span className={styles.result} id="result">
                  {result}
                </span>
              </div>
              {movie ? (
                <div>
                  <button
                    className={styles.deleteButton}
                    onClick={handleDelete}
                  >
                    delete
                  </button>
                </div>
              ) : null}
            </div>
          </form>
        </div>
      </div>
    </>,
    document.getElementById("portal")!
  );
};

export default CreateMovieModal;
